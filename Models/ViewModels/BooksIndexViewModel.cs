using LibraryManagementSystem.Data.Migrations;

namespace LibraryManagementSystem.Models.ViewModels
{
    public class BooksIndexViewModel
    {
        public string UserId { get; set; }

        public IEnumerable<BorrowRequest> BookRequests { get; set; }
        public IEnumerable<BorrowingHistory> BorrowHistory { get; set; }
        public IEnumerable<Book> Books { get; set; }
    }
}
