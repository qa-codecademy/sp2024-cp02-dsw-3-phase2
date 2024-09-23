using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtShop.DataAcces.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Users_BoughtByUserId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Users_SoldByUserId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Users_UserId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_BoughtByUserId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_SoldByUserId",
                table: "Images");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Users_UserId",
                table: "Images",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Users_UserId",
                table: "Images");

            migrationBuilder.CreateIndex(
                name: "IX_Images_BoughtByUserId",
                table: "Images",
                column: "BoughtByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_SoldByUserId",
                table: "Images",
                column: "SoldByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Users_BoughtByUserId",
                table: "Images",
                column: "BoughtByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Users_SoldByUserId",
                table: "Images",
                column: "SoldByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Users_UserId",
                table: "Images",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
