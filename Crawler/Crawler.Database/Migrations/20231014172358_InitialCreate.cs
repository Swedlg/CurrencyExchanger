using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crawler.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LatestUploadDates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UploadDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LatestUploadDates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LatestUploadDates_UploadDate",
                table: "LatestUploadDates",
                column: "UploadDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LatestUploadDates");
        }
    }
}
