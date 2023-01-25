using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinanceApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddColorToCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "IncomeCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "ExpenseCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "IncomeCategories");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "ExpenseCategories");
        }
    }
}
