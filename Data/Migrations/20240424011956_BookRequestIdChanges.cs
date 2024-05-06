using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class BookRequestIdChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BorrowRequests",
                table: "BorrowRequests");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "BorrowRequests",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BorrowRequests",
                table: "BorrowRequests",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowRequests_BookId",
                table: "BorrowRequests",
                column: "BookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BorrowRequests",
                table: "BorrowRequests");

            migrationBuilder.DropIndex(
                name: "IX_BorrowRequests_BookId",
                table: "BorrowRequests");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BorrowRequests");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BorrowRequests",
                table: "BorrowRequests",
                columns: new[] { "BookId", "UserId" });
        }
    }
}
