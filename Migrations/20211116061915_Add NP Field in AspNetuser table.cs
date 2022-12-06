using Microsoft.EntityFrameworkCore.Migrations;

namespace Meeting_App.Migrations
{
    public partial class AddNPFieldinAspNetusertable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NP",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NP",
                table: "AspNetUsers");
        }
    }
}
