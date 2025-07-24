using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcademiaHub.Migrations
{
    /// <inheritdoc />
    public partial class FormStudentAnswers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "b985b240-2dce-4365-bcf5-c4c792b9076b", "ae11a8b9-658e-4079-b6e8-8f3959de2805" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ae11a8b9-658e-4079-b6e8-8f3959de2805");

            migrationBuilder.CreateTable(
                name: "formStudentAnswers",
                columns: table => new
                {
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_formStudentAnswers", x => new { x.StudentId, x.FormId, x.QuestionId });
                    table.ForeignKey(
                        name: "FK_formStudentAnswers_QuestionsForms_FormId",
                        column: x => x.FormId,
                        principalTable: "QuestionsForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_formStudentAnswers_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_formStudentAnswers_questionBank_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "questionBank",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_formStudentAnswers_FormId",
                table: "formStudentAnswers",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_formStudentAnswers_QuestionId",
                table: "formStudentAnswers",
                column: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "formStudentAnswers");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "ae11a8b9-658e-4079-b6e8-8f3959de2805", 0, "ae11a8b9-658e-4079-b6e8-8f3959de2805", "Admin@AcademiaHub.com", true, false, null, null, "ADMIN@ACADEMIAHUB.COM", "AQAAAAIAAYagAAAAEIVv8Ew176UOLzUwHnmDRvu0KcPB5Uon3GVg3h38rQ7FzhERZjU9Tdr7T4qgP7vxHg==", null, false, "bdaef782-7658-4e17-80c5-ab1988038b52", false, "Admin@AcademiaHub.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "b985b240-2dce-4365-bcf5-c4c792b9076b", "ae11a8b9-658e-4079-b6e8-8f3959de2805" });
        }
    }
}
