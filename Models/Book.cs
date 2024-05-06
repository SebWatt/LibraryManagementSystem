using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Author { get; set; }
        public bool IsArchived { get; set; }


        // FK
        public string? CurrentApplicationUserId { get; set; }

        // Nav 
        public virtual ICollection<BorrowRequest>? BorrowRequests { get; set; }
        public virtual ICollection<BorrowingHistory> BorrowingHistories { get; set; }
        public virtual ApplicationUser? CurrentApplicationUser { get; set; }
    }
}
