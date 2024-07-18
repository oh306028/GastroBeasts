using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Migrations
{
    /// <inheritdoc />
    public partial class OnDeleteRestrictForRestaurantcategoriesTableAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantCategories_Categories_CategoryId",
                table: "RestaurantCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantCategories_Restaurants_RestaurantId",
                table: "RestaurantCategories");

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantCategories_Categories_CategoryId",
                table: "RestaurantCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantCategories_Restaurants_RestaurantId",
                table: "RestaurantCategories",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantCategories_Categories_CategoryId",
                table: "RestaurantCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantCategories_Restaurants_RestaurantId",
                table: "RestaurantCategories");

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantCategories_Categories_CategoryId",
                table: "RestaurantCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantCategories_Restaurants_RestaurantId",
                table: "RestaurantCategories",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
