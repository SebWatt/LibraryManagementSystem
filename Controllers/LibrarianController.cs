using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Migrations;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Roles = Constants.SuperAdminRole + "," + Constants.LibrarianRole)]
    public class LibrarianController : Controller
    {
        private readonly ILogger<LibrarianController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public LibrarianController(ILogger<LibrarianController> logger, UserManager<ApplicationUser> um, ApplicationDbContext context, RoleManager<IdentityRole> rm)
        {
            _logger = logger;
            _userManager = um;
            _context = context;
            _roleManager = rm;
        }
        public IActionResult Index()
        {
            var books = _context.Books.ToList();
            return View(books);
        }

        public IActionResult RequestManager()
        {
            var borrowRequests = _context.BorrowRequests.ToList();
            return View(borrowRequests);
        }

        public IActionResult BookBorrowingHistory(int bookId) 
        {
            List<BorrowingHistory> bookHistory = _context.BorrowingHistories.Where(bh => bh.BookId == bookId).ToList();

            return View(bookHistory);
        }

        [HttpPost] 
        public IActionResult ApproveRequest(int bookId, string userId, int id)
        {
            BorrowRequest? borrowRequest = _context.BorrowRequests.FirstOrDefault(br => !br.Equals(null) && br.Id == id);
            ApplicationUser? applicationUser = _context.ApplicationUsers.Find(userId); 
            Book? book = _context.Books.Find(bookId);

            if (book.CurrentApplicationUserId == null)
            {
                if (borrowRequest != null)
                {
                    borrowRequest.IsApproved = "approved";
                    book.CurrentApplicationUserId = userId;

                    BorrowingHistory borrowingHistory = new BorrowingHistory()
                    {
                        BookId = bookId,
                        UserId = userId,
                        BorrowedDate = DateTime.Now
                    };
                    _context.BorrowingHistories.Add(borrowingHistory);

                    _context.SaveChanges();
                }

                return RedirectToAction("RequestManager");
            }
            else {
                return Forbid();
            }
        }

        [HttpPost]
        public IActionResult DenyRequest(int id, string userId)
        {
            BorrowRequest? borrowRequest = _context.BorrowRequests.FirstOrDefault(br => !br.Equals(null) && br.Id == id);
            ApplicationUser? applicationUser = _context.ApplicationUsers.Find(userId);
           
            borrowRequest.IsApproved = "denied";

            _context.SaveChanges();
    
            return RedirectToAction("RequestManager");
        }

        [HttpPost]
        public IActionResult ReturnBook(int bookId, string userId, int id)
        {
            BorrowRequest? borrowRequest = _context.BorrowRequests.FirstOrDefault(br => !br.Equals(null) && br.Id == id);
            ApplicationUser? applicationUser = _context.ApplicationUsers.Find(userId);
            Book? returnedBook = _context.Books.Find(bookId);
            BorrowingHistory? borrowingHistory = _context.BorrowingHistories.FirstOrDefault(br => br.BookId == bookId && br.UserId == userId && br.ReturnedDate == DateTime.MinValue);

            borrowingHistory.ReturnedDate = DateTime.Now;
            borrowRequest.IsApproved = "returned";
            returnedBook.CurrentApplicationUserId = null;

            _context.SaveChanges();

            return RedirectToAction("RequestManager");
        }

        [HttpPost]
        public IActionResult AddNewBook(string bookTitle, string bookGenre, string bookAuthor)
        {
            if (!string.IsNullOrEmpty(bookTitle) && !string.IsNullOrEmpty(bookGenre) && !string.IsNullOrEmpty(bookAuthor))
            {
                var newBook = new Book
                {
                    Name = bookTitle,
                    Genre = bookGenre,
                    Author = bookAuthor,
                    IsArchived = false
                };

                _context.Books.Add(newBook);
                _context.SaveChanges();


                return RedirectToAction("Index");
            }
            else
            {
                return Forbid();
            }
        }

        [HttpPost]
        public IActionResult ArchiveBook(int bookId)
        { 
            Book? bookToArchive = _context.Books.Find(bookId);
            List<BorrowRequest> borrowRequests = _context.BorrowRequests.Where(b => b.BookId  == bookId).ToList();

            bookToArchive.IsArchived = true;

            if(borrowRequests.Any())
            {
                foreach (var borrowRequest in borrowRequests)
                {
                    borrowRequest.IsApproved = "denied";
                }
            }

            if (bookToArchive.CurrentApplicationUserId != null)
            {
                BorrowingHistory? borrowingHistory = _context.BorrowingHistories
                    .FirstOrDefault(bh => bh.UserId == bookToArchive.CurrentApplicationUserId && bh.BookId == bookId && bh.ReturnedDate == DateTime.MinValue);
                // Borrow request of the user who had the book before it was archived.
                BorrowRequest userWhoLoanedBRWhenArchived = borrowRequests.FirstOrDefault(br => br.UserId == bookToArchive.CurrentApplicationUserId);

                userWhoLoanedBRWhenArchived.IsApproved = "returned";

                borrowingHistory.ReturnedDate = DateTime.Now;
                bookToArchive.CurrentApplicationUserId = null;
            }


            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UnArchiveBook(int bookId)
        {
            Book? bookToArchive = _context.Books.Find(bookId);

            bookToArchive.IsArchived = false;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
