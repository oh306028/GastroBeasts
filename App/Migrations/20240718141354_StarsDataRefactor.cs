using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Migrations
{
    /// <inheritdoc />
    public partial class StarsDataRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Rating",
                table: "Stars",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Stars",
                keyColumn: "Id",
                keyValue: 1,
                column: "Rating",
                value: "GastroLoser");

            migrationBuilder.UpdateData(
                table: "Stars",
                keyColumn: "Id",
                keyValue: 2,
                column: "Rating",
                value: "Low");

            migrationBuilder.UpdateData(
                table: "Stars",
                keyColumn: "Id",
                keyValue: 3,
                column: "Rating",
                value: "Medium");

            migrationBuilder.UpdateData(
                table: "Stars",
                keyColumn: "Id",
                keyValue: 4,
                column: "Rating",
                value: "High");

            migrationBuilder.UpdateData(
                table: "Stars",
                keyColumn: "Id",
                keyValue: 5,
                column: "Rating",
                value: "GastroBeast");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Stars");
        }
    }
}
