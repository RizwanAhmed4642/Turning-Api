using Microsoft.EntityFrameworkCore.Migrations;

namespace Meeting_App.Migrations
{
    public partial class NewCoulmen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "AspNetUsers");

            
        }
    }
}
