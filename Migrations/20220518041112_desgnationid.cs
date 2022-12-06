using Microsoft.EntityFrameworkCore.Migrations;

namespace Meeting_App.Migrations
{
    public partial class desgnationid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContactDesignationId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactDesignationId",
                table: "AspNetUsers");
        }
    }
}
