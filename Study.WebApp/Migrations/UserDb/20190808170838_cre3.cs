using Microsoft.EntityFrameworkCore.Migrations;

namespace Study.WebApp.Migrations.UserDb
{
    public partial class cre3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Desc",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Desc",
                table: "AspNetUsers");
        }
    }
}
