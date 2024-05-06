using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    public class LibraryController : Controller
    {

        private readonly ILogger<LibraryController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LibraryController(ILogger<LibraryController> logger, UserManager<ApplicationUser> um, ApplicationDbContext context, RoleManager<IdentityRole> rm, SignInManager<ApplicationUser> sim)
        {
            _logger = logger;
            _userManager = um;
            _context = context;
            _roleManager = rm;
            _signInManager = sim;
        }

        [AllowAnonymous]
        public IActionResult Index(string userSearchQuery)
        {
            ApplicationUser? user = GetUser();
            string userId = user?.Id ?? string.Empty;
            var books = _context.Books.Where(b => !b.IsArchived);

            if (!string.IsNullOrWhiteSpace(userSearchQuery))
            {
                books.Where(b =>
                    b.Name.Contains(userSearchQuery, StringComparison.OrdinalIgnoreCase) ||
                    b.Genre.Contains(userSearchQuery, StringComparison.OrdinalIgnoreCase) ||
                    b.Author.Contains(userSearchQuery, StringComparison.OrdinalIgnoreCase));
            }

            var bookRequests = _context.BorrowRequests.Where(br => br.UserId == userId && br.IsApproved == "pending" || br.IsApproved == "approved").ToList();

            BooksIndexViewModel vm = new()
            {
                Books = books.ToList(),
                BookRequests = bookRequests,
                UserId = user?.Id ?? string.Empty,
            };
            return View(vm);
        }
        
        [Authorize(Roles = Constants.SuperAdminRole + "," + Constants.BorrowerRole)]
        public async Task<IActionResult> MakeRequest(int id)
        {
            ApplicationUser? user = GetUser();
            Book? book = _context.Books.FirstOrDefault(b => b.Id == id);

            if (user == null)
                return Forbid();

            if (book == null)
                return NotFound();

            if (book.CurrentApplicationUserId != null)
                return Forbid();

            BorrowRequest borrowRequest = new()
            {
                DateTime = DateTime.Now,
                IsApproved = "pending",
                BookId = id,
                UserId = user.Id
            };

            _context.BorrowRequests.Add(borrowRequest);
            _context.SaveChanges();

            await RefreshSignIn(user);
            return RedirectToAction("Index");
        }

        public IActionResult MyBookRequests()
        {
            ApplicationUser? user = GetUser();
            string userId = user?.Id ?? string.Empty;
            var bookRequests = _context.BorrowRequests.Where(br => br.UserId == userId).ToList();
            return View(bookRequests);
        }

        private ApplicationUser? GetUser()
        {
            string? userId = _userManager.GetUserId(User);
            return _context.Users.FirstOrDefault(u => u.Id == userId);
        }

        public async Task<IActionResult> RefreshSignIn(ApplicationUser user)
        {
            // Sign out the current user
            await _signInManager.SignOutAsync();

            // Sign in the user again
            await _signInManager.SignInAsync(user, isPersistent: false);

            // Redirect to a desired page after sign-in
            return RedirectToAction("Index");
        }


    }
}
