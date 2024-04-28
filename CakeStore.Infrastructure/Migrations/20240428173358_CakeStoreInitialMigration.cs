using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CakeStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CakeStoreInitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "CakeStore");
            
            migrationBuilder.CreateTable(
                name: "Errors",
                schema: "CakeStore",
                columns: table => new
                {
                    Code = table.Column<short>(type: "smallint", nullable: false),
                    LanguageId = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Message = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    HttpStatusCode = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Errors", x => new { x.Code, x.LanguageId });
                    table.ForeignKey(
                        name: "FK_Errors_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "References",
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cakes",
                schema: "CakeStore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cakes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CakeToUsers",
                schema: "CakeStore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CakeId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CakeToUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CakeToUsers_Cakes_CakeId",
                        column: x => x.CakeId,
                        principalSchema: "CakeStore",
                        principalTable: "Cakes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                schema: "CakeStore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CakeId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Cakes_CakeId",
                        column: x => x.CakeId,
                        principalSchema: "CakeStore",
                        principalTable: "Cakes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Reactions",
                schema: "CakeStore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CakeId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reactions_Cakes_CakeId",
                        column: x => x.CakeId,
                        principalSchema: "CakeStore",
                        principalTable: "Cakes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                schema: "CakeStore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CakeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Cakes_CakeId",
                        column: x => x.CakeId,
                        principalSchema: "CakeStore",
                        principalTable: "Cakes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cakes_ImageId",
                schema: "CakeStore",
                table: "Cakes",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_CakeToUsers_CakeId",
                schema: "CakeStore",
                table: "CakeToUsers",
                column: "CakeId");

            migrationBuilder.CreateIndex(
                name: "IX_Errors_LanguageId",
                schema: "CakeStore",
                table: "Errors",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_CakeId",
                schema: "CakeStore",
                table: "Images",
                column: "CakeId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_CakeId",
                schema: "CakeStore",
                table: "Reactions",
                column: "CakeId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_UserId_CakeId",
                schema: "CakeStore",
                table: "Reactions",
                columns: new[] { "UserId", "CakeId" });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CakeId",
                schema: "CakeStore",
                table: "Reviews",
                column: "CakeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cakes_Images_ImageId",
                schema: "CakeStore",
                table: "Cakes",
                column: "ImageId",
                principalSchema: "CakeStore",
                principalTable: "Images",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cakes_Images_ImageId",
                schema: "CakeStore",
                table: "Cakes");

            migrationBuilder.DropTable(
                name: "CakeToUsers",
                schema: "CakeStore");

            migrationBuilder.DropTable(
                name: "Errors",
                schema: "CakeStore");

            migrationBuilder.DropTable(
                name: "Reactions",
                schema: "CakeStore");

            migrationBuilder.DropTable(
                name: "Reviews",
                schema: "CakeStore");

            migrationBuilder.DropTable(
                name: "Images",
                schema: "CakeStore");

            migrationBuilder.DropTable(
                name: "Cakes",
                schema: "CakeStore");
        }
    }
}
