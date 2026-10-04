using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TerminologyService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Elements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    VersionRefBookId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefBooks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefBooks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VersionRefBooks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RefBookId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Version = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VersionRefBooks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Elements_VersionRefBookId_Code",
                table: "Elements",
                columns: new[] { "VersionRefBookId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefBooks_Code",
                table: "RefBooks",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VersionRefBooks_RefBookId_Date",
                table: "VersionRefBooks",
                columns: new[] { "RefBookId", "Date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VersionRefBooks_RefBookId_Version",
                table: "VersionRefBooks",
                columns: new[] { "RefBookId", "Version" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Elements");

            migrationBuilder.DropTable(
                name: "RefBooks");

            migrationBuilder.DropTable(
                name: "VersionRefBooks");
        }
    }
}
