namespace LibraryManagementSystem.Models
{
    public class BorrowRequest
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string IsApproved { get; set; }

        //fk 
        public int BookId { get; set; }
        public string? UserId { get; set; }
        
        // nav 
        public virtual ApplicationUser? CurrentApplicationUser { get; set; }
        public virtual Book? Book { get; set; }
    }
}
