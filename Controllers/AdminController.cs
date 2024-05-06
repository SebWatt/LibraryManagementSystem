using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Roles = Constants.SuperAdminRole + "," + Constants.AdminRole) ]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        
       public AdminController(UserManager<ApplicationUser> um, RoleManager<IdentityRole> rm)
       {
            _userManager = um;
            _roleManager = rm;
       }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles()
        {
            ReviewUsersViewModel vm = new()
            {
                Roles = _roleManager.Roles.Select(r => r.Name).ToList()!
            };

            foreach (var user in _userManager.Users)
            {
                vm.Members.Add(
                    new ReviewUsersViewModel.Member()
                    {
                        Name = user.UserName ?? "no user name",
                        Roles = (await _userManager.GetRolesAsync(user)).ToHashSet()
                    }
                    );
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleRole(string roleName, string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound($"User '{userName}' not found.");
            }


            if (User.IsInRole(Constants.AdminRole) && !User.IsInRole(Constants.SuperAdminRole) && (roleName == Constants.AdminRole || roleName == Constants.SuperAdminRole || (await _userManager.IsInRoleAsync(user, Constants.AdminRole) || await _userManager.IsInRoleAsync(user, Constants.SuperAdminRole))))
            {
                return Forbid(); 
            }

            var userIsInRole = await _userManager.IsInRoleAsync(user, roleName);
            if (userIsInRole)
            {
                if (roleName == Constants.SuperAdminRole && User.Identity.Name == userName)
                {
                    // Super admin is trying to remove their own super admin role
                    ViewBag.WarningMessage = "You are about to remove your super admin role. This action cannot be undone.";
                    return View("ConfirmSuperAdminRoleRemoval");
                }
                // If the user is already in the role, remove the role
                await _userManager.RemoveFromRoleAsync(user, roleName);
            }
            else
            {
                // If the user is not in the role, add the role
                await _userManager.AddToRoleAsync(user, roleName);
            }
            return RedirectToAction("ManageUserRoles");
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmRoleRemoval(string roleName, string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound($"User '{userName}' not found.");
            }

            var userIsInRole = await _userManager.IsInRoleAsync(user, roleName);
            await _userManager.RemoveFromRoleAsync(user, roleName);

            return RedirectToAction("ManageUserRoles");
        }
    }
}
