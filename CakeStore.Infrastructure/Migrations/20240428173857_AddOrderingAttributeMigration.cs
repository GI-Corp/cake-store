using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CakeStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderingAttributeMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Ordering",
                schema: "CakeStore",
                table: "Images",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Ordering",
                schema: "CakeStore",
                table: "Cakes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ordering",
                schema: "CakeStore",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Ordering",
                schema: "CakeStore",
                table: "Cakes");
        }
    }
}
