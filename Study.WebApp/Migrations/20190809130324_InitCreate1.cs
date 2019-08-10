using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Study.WebApp.Migrations
{
    public partial class InitCreate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticalClassifications",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    AddDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticalClassifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArticalComments",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ArticalId = table.Column<string>(nullable: false),
                    CommenterId = table.Column<string>(nullable: false),
                    CommentContent = table.Column<string>(nullable: false),
                    CommentTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticalComments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Articals",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ClassificationId = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    PublishTime = table.Column<DateTime>(nullable: false),
                    VisitCount = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassBases",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassBases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentTasks",
                columns: table => new
                {
                    TaskId = table.Column<string>(nullable: false),
                    StudentId = table.Column<string>(nullable: false),
                    StudentAnswer = table.Column<string>(nullable: false),
                    Grade = table.Column<float>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    TeacherReply = table.Column<string>(nullable: true),
                    IsTeacherModified = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentTasks", x => new { x.TaskId, x.StudentId });
                    table.UniqueConstraint("AK_StudentTasks_StudentId_TaskId", x => new { x.StudentId, x.TaskId });
                });

            migrationBuilder.CreateTable(
                name: "TeacherClasses",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    TeacherId = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    AddDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherClasses", x => new { x.Id, x.TeacherId });
                });

            migrationBuilder.CreateTable(
                name: "TeacherCourses",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    TeacherId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Desc = table.Column<string>(nullable: true),
                    AddDateTime = table.Column<DateTime>(nullable: false),
                    LastModifyDatetime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherCourses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeacherHelpers",
                columns: table => new
                {
                    TeacherId = table.Column<string>(nullable: false),
                    HelpId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherHelpers", x => new { x.TeacherId, x.HelpId });
                    table.UniqueConstraint("AK_TeacherHelpers_HelpId_TeacherId", x => new { x.HelpId, x.TeacherId });
                });

            migrationBuilder.CreateTable(
                name: "TeacherTasks",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    TeacherId = table.Column<string>(nullable: false),
                    HelperId = table.Column<string>(nullable: true),
                    CourseId = table.Column<string>(nullable: false),
                    TaskName = table.Column<string>(nullable: false),
                    TaskContent = table.Column<string>(nullable: false),
                    TaskWriteTime = table.Column<DateTime>(nullable: false),
                    TaskStartTime = table.Column<DateTime>(nullable: false),
                    TaskEndTime = table.Column<DateTime>(nullable: false),
                    ClassIds = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherTasks", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ArticalClassifications",
                columns: new[] { "Id", "AddDateTime", "Name" },
                values: new object[] { "1F1E7427-977C-4922-B450-7B63784F24FE", new DateTime(2019, 8, 9, 21, 3, 23, 837, DateTimeKind.Local).AddTicks(8948), "系统公告" });

            migrationBuilder.InsertData(
                table: "ArticalClassifications",
                columns: new[] { "Id", "AddDateTime", "Name" },
                values: new object[] { "2F1E7427-977C-4922-B450-7B63784F24FE", new DateTime(2019, 8, 9, 21, 3, 23, 840, DateTimeKind.Local).AddTicks(5760), "Asp.Net" });

            migrationBuilder.InsertData(
                table: "ArticalClassifications",
                columns: new[] { "Id", "AddDateTime", "Name" },
                values: new object[] { "221E7427-977C-4922-B450-7B63784F24FE", new DateTime(2019, 8, 9, 21, 3, 23, 840, DateTimeKind.Local).AddTicks(5768), "数据库" });

            migrationBuilder.InsertData(
                table: "ArticalClassifications",
                columns: new[] { "Id", "AddDateTime", "Name" },
                values: new object[] { "221E5427-977C-4922-B450-7B63784F24FE", new DateTime(2019, 8, 9, 21, 3, 23, 840, DateTimeKind.Local).AddTicks(5770), "数据结构与算法" });

            migrationBuilder.InsertData(
                table: "ArticalClassifications",
                columns: new[] { "Id", "AddDateTime", "Name" },
                values: new object[] { "221E5427-977C-4922-B450-7B63784F25FE", new DateTime(2019, 8, 9, 21, 3, 23, 840, DateTimeKind.Local).AddTicks(5770), "计算机网络" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticalClassifications");

            migrationBuilder.DropTable(
                name: "ArticalComments");

            migrationBuilder.DropTable(
                name: "Articals");

            migrationBuilder.DropTable(
                name: "ClassBases");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "StudentTasks");

            migrationBuilder.DropTable(
                name: "TeacherClasses");

            migrationBuilder.DropTable(
                name: "TeacherCourses");

            migrationBuilder.DropTable(
                name: "TeacherHelpers");

            migrationBuilder.DropTable(
                name: "TeacherTasks");
        }
    }
}
