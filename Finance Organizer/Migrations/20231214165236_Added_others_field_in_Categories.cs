using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finance_Organizer.Migrations
{
    /// <inheritdoc />
    public partial class Added_others_field_in_Categories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Others",
                table: "Categories",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Others",
                table: "Categories");
        }
    }
}
