using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class updateRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8ee06f05-c4ea-407b-80fc-e4284a2300af");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "94464dfb-10e4-457a-9918-1efe14c1de28");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3e4f9aaf-47d0-4e0b-ba7a-aac2b9d57ab7", null, "customer", "CUSTOMER" },
                    { "9fc782a7-f506-4d77-85bf-939cf8faf493", null, "admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e4f9aaf-47d0-4e0b-ba7a-aac2b9d57ab7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9fc782a7-f506-4d77-85bf-939cf8faf493");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8ee06f05-c4ea-407b-80fc-e4284a2300af", null, "user", "USER" },
                    { "94464dfb-10e4-457a-9918-1efe14c1de28", null, "admin", "ADMIN" }
                });
        }
    }
}
