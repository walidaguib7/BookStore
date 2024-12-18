using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class updateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "625485a7-5475-4a62-ad54-ed303fd4d49e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "79eecab1-ddc1-4752-bd17-316205e00f80");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "416bc3a5-ecbc-4ce5-b82e-166e790d4944", null, "customer", "CUSTOMER" },
                    { "89739a55-0793-4988-a040-4d642259a966", null, "admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "416bc3a5-ecbc-4ce5-b82e-166e790d4944");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "89739a55-0793-4988-a040-4d642259a966");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "625485a7-5475-4a62-ad54-ed303fd4d49e", null, "admin", "ADMIN" },
                    { "79eecab1-ddc1-4752-bd17-316205e00f80", null, "customer", "CUSTOMER" }
                });
        }
    }
}
