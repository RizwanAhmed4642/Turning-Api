using Microsoft.EntityFrameworkCore.Migrations;

namespace Meeting_App.Migrations
{
    public partial class catidcompanyid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContactCategoryId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContactCompanyId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContactDepartmentId",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactCategoryId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ContactCompanyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ContactDepartmentId",
                table: "AspNetUsers");
        }
    }
}
