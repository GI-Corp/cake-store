using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialSharedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "References");

            migrationBuilder.CreateTable(
                name: "Languages",
                schema: "References",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Languages",
                schema: "References");
        }
    }
}
