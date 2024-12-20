using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class addedFilesToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "03e08ba7-9f3d-44ab-a3f1-a9fd9b04b8f3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "186b374a-cd4b-4ca1-b4c7-14ccc4e0c6ec");

            migrationBuilder.AddColumn<int>(
                name: "mediaId",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b8e6eea1-846a-423b-948a-c65f3dcc4b2d", null, "admin", "ADMIN" },
                    { "bc40200c-7160-41d6-bf20-91c2e033da54", null, "customer", "CUSTOMER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_mediaId",
                table: "AspNetUsers",
                column: "mediaId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_media_mediaId",
                table: "AspNetUsers",
                column: "mediaId",
                principalTable: "media",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_media_mediaId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_mediaId",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b8e6eea1-846a-423b-948a-c65f3dcc4b2d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bc40200c-7160-41d6-bf20-91c2e033da54");

            migrationBuilder.DropColumn(
                name: "mediaId",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "03e08ba7-9f3d-44ab-a3f1-a9fd9b04b8f3", null, "admin", "ADMIN" },
                    { "186b374a-cd4b-4ca1-b4c7-14ccc4e0c6ec", null, "customer", "CUSTOMER" }
                });
        }
    }
}
