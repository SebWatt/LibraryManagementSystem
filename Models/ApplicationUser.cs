using Microsoft.AspNetCore.Identity;

namespace LibraryManagementSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<BorrowRequest> BorrowRequests { get; set; }
        public virtual ICollection<BorrowingHistory> BorrowingHistories { get; set; }
        public virtual ICollection<Book> BorrowedBooks { get; set; }
    }
}
