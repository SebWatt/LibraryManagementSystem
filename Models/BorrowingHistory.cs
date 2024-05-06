namespace LibraryManagementSystem.Models
{
    public class BorrowingHistory
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string UserId { get; set; }
        public DateTime BorrowedDate { get; set; }
        public DateTime ReturnedDate { get; set; }

        // Navigation properties
        public virtual Book Book { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}