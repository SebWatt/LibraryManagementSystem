using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBooksArchiveAndHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_AspNetUsers_CurrentApplicationUserId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_BorrowRequests_AspNetUsers_UserId",
                table: "BorrowRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_BorrowRequests_Books_BookId",
                table: "BorrowRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BorrowRequests",
                table: "BorrowRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Books",
                table: "Books");

            migrationBuilder.RenameTable(
                name: "BorrowRequests",
                newName: "BorrowRequest");

            migrationBuilder.RenameTable(
                name: "Books",
                newName: "Book");

            migrationBuilder.RenameIndex(
                name: "IX_BorrowRequests_UserId",
                table: "BorrowRequest",
                newName: "IX_BorrowRequest_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Books_CurrentApplicationUserId",
                table: "Book",
                newName: "IX_Book_CurrentApplicationUserId");

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Book",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BorrowRequest",
                table: "BorrowRequest",
                columns: new[] { "BookId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Book",
                table: "Book",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_AspNetUsers_CurrentApplicationUserId",
                table: "Book",
                column: "CurrentApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowRequest_AspNetUsers_UserId",
                table: "BorrowRequest",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowRequest_Book_BookId",
                table: "BorrowRequest",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_AspNetUsers_CurrentApplicationUserId",
                table: "Book");

            migrationBuilder.DropForeignKey(
                name: "FK_BorrowRequest_AspNetUsers_UserId",
                table: "BorrowRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_BorrowRequest_Book_BookId",
                table: "BorrowRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BorrowRequest",
                table: "BorrowRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Book",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Book");

            migrationBuilder.RenameTable(
                name: "BorrowRequest",
                newName: "BorrowRequests");

            migrationBuilder.RenameTable(
                name: "Book",
                newName: "Books");

            migrationBuilder.RenameIndex(
                name: "IX_BorrowRequest_UserId",
                table: "BorrowRequests",
                newName: "IX_BorrowRequests_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Book_CurrentApplicationUserId",
                table: "Books",
                newName: "IX_Books_CurrentApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BorrowRequests",
                table: "BorrowRequests",
                columns: new[] { "BookId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Books",
                table: "Books",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_AspNetUsers_CurrentApplicationUserId",
                table: "Books",
                column: "CurrentApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowRequests_AspNetUsers_UserId",
                table: "BorrowRequests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowRequests_Books_BookId",
                table: "BorrowRequests",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
