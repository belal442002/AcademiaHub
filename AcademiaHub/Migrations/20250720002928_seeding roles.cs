using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AcademiaHub.Migrations
{
    /// <inheritdoc />
    public partial class seedingroles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "26c2b87c-bd59-48aa-87b6-34b414f8d12e", "26c2b87c-bd59-48aa-87b6-34b414f8d12e", "Student", "STUDENT" },
                    { "b985b240-2dce-4365-bcf5-c4c792b9076b", "b985b240-2dce-4365-bcf5-c4c792b9076b", "Admin", "ADMIN" },
                    { "f637afd6-a47d-44fc-84bc-fdbca6ed2e4d", "f637afd6-a47d-44fc-84bc-fdbca6ed2e4d", "Teacher", "TEACHER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "26c2b87c-bd59-48aa-87b6-34b414f8d12e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b985b240-2dce-4365-bcf5-c4c792b9076b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f637afd6-a47d-44fc-84bc-fdbca6ed2e4d");
        }
    }
}
