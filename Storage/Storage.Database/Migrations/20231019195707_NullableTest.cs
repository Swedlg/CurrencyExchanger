using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Storage.Database.Migrations
{
    /// <inheritdoc />
    public partial class NullableTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrencyValueByDates_СurrencyInfos_BaseCurrencyId",
                table: "CurrencyValueByDates");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrencyValueByDates_СurrencyInfos_CurrencyId",
                table: "CurrencyValueByDates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrencyValueByDates",
                table: "CurrencyValueByDates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_СurrencyInfos",
                table: "СurrencyInfos");

            migrationBuilder.RenameTable(
                name: "CurrencyValueByDates",
                newName: "currencyvaluebydates");

            migrationBuilder.RenameTable(
                name: "СurrencyInfos",
                newName: "currencyinfos");

            migrationBuilder.RenameIndex(
                name: "IX_CurrencyValueByDates_CurrencyId",
                table: "currencyvaluebydates",
                newName: "IX_currencyvaluebydates_CurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_CurrencyValueByDates_BaseCurrencyId",
                table: "currencyvaluebydates",
                newName: "IX_currencyvaluebydates_BaseCurrencyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_currencyvaluebydates",
                table: "currencyvaluebydates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_currencyinfos",
                table: "currencyinfos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_currencyvaluebydates_currencyinfos_BaseCurrencyId",
                table: "currencyvaluebydates",
                column: "BaseCurrencyId",
                principalTable: "currencyinfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_currencyvaluebydates_currencyinfos_CurrencyId",
                table: "currencyvaluebydates",
                column: "CurrencyId",
                principalTable: "currencyinfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_currencyvaluebydates_currencyinfos_BaseCurrencyId",
                table: "currencyvaluebydates");

            migrationBuilder.DropForeignKey(
                name: "FK_currencyvaluebydates_currencyinfos_CurrencyId",
                table: "currencyvaluebydates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_currencyvaluebydates",
                table: "currencyvaluebydates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_currencyinfos",
                table: "currencyinfos");

            migrationBuilder.RenameTable(
                name: "currencyvaluebydates",
                newName: "CurrencyValueByDates");

            migrationBuilder.RenameTable(
                name: "currencyinfos",
                newName: "СurrencyInfos");

            migrationBuilder.RenameIndex(
                name: "IX_currencyvaluebydates_CurrencyId",
                table: "CurrencyValueByDates",
                newName: "IX_CurrencyValueByDates_CurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_currencyvaluebydates_BaseCurrencyId",
                table: "CurrencyValueByDates",
                newName: "IX_CurrencyValueByDates_BaseCurrencyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrencyValueByDates",
                table: "CurrencyValueByDates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_СurrencyInfos",
                table: "СurrencyInfos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrencyValueByDates_СurrencyInfos_BaseCurrencyId",
                table: "CurrencyValueByDates",
                column: "BaseCurrencyId",
                principalTable: "СurrencyInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrencyValueByDates_СurrencyInfos_CurrencyId",
                table: "CurrencyValueByDates",
                column: "CurrencyId",
                principalTable: "СurrencyInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
