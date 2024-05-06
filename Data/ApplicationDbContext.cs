using LibraryManagementSystem.Models;
using LibraryManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace LibraryManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<BorrowingHistory> BorrowingHistories { get; set; }
        public virtual DbSet<BorrowRequest> BorrowRequests { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);


            mb.Entity<Book>()
                .HasOne(b => b.CurrentApplicationUser)
                .WithMany(a => a.BorrowedBooks)
                .HasForeignKey(b => b.CurrentApplicationUserId)
                .IsRequired(false);

            mb.Entity<BorrowRequest>()
                .HasKey(br => br.Id);

            mb.Entity<BorrowRequest>()
                .HasOne(br => br.Book)
                .WithMany(b => b.BorrowRequests)
                .HasForeignKey(br => br.BookId)
                .IsRequired();

            mb.Entity<BorrowRequest>()
                .HasOne(br => br.CurrentApplicationUser)
                .WithMany(u => u.BorrowRequests)
                .HasForeignKey(br => br.UserId)
                .IsRequired();

            mb.Entity<BorrowingHistory>()
                .HasKey(bh => bh.Id);

            mb.Entity<BorrowingHistory>()
                .HasOne(bh => bh.Book)
                .WithMany(b => b.BorrowingHistories)
                .HasForeignKey(bh => bh.BookId);

            mb.Entity<BorrowingHistory>()
                .HasOne(bh => bh.User)
                .WithMany(u => u.BorrowingHistories)
                .HasForeignKey(bh => bh.UserId);
        }
    }
}
