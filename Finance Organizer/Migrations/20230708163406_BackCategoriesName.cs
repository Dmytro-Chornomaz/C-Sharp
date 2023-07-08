using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finance_Organizer.Migrations
{
    /// <inheritdoc />
    public partial class BackCategoriesName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_TheCategories_CategoriesId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TheCategories",
                table: "TheCategories");

            migrationBuilder.RenameTable(
                name: "TheCategories",
                newName: "Categories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Categories_CategoriesId",
                table: "Transactions",
                column: "CategoriesId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Categories_CategoriesId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "TheCategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TheCategories",
                table: "TheCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_TheCategories_CategoriesId",
                table: "Transactions",
                column: "CategoriesId",
                principalTable: "TheCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
