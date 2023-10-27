using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Storage.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "currencyinfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    EngName = table.Column<string>(type: "text", nullable: false),
                    RId = table.Column<string>(type: "text", nullable: false),
                    ISOCharCode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_currencyinfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "currencyvaluebydates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BaseCurrencyId = table.Column<int>(type: "integer", nullable: false),
                    CurrencyId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_currencyvaluebydates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_currencyvaluebydates_currencyinfos_BaseCurrencyId",
                        column: x => x.BaseCurrencyId,
                        principalTable: "currencyinfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_currencyvaluebydates_currencyinfos_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "currencyinfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_currencyvaluebydates_BaseCurrencyId",
                table: "currencyvaluebydates",
                column: "BaseCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_currencyvaluebydates_CurrencyId",
                table: "currencyvaluebydates",
                column: "CurrencyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "currencyvaluebydates");

            migrationBuilder.DropTable(
                name: "currencyinfos");
        }
    }
}
