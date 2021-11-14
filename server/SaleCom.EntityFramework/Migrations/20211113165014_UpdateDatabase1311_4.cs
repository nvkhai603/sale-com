using Microsoft.EntityFrameworkCore.Migrations;

namespace SaleCom.EntityFramework.Migrations
{
    public partial class UpdateDatabase1311_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_authentication_tickets_users_UserId",
                table: "authentication_tickets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_authentication_tickets_users_UserId",
                table: "authentication_tickets",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
