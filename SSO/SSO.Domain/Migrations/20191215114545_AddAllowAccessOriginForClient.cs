using Microsoft.EntityFrameworkCore.Migrations;

namespace SSO.Domain.Migrations
{
    public partial class AddAllowAccessOriginForClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AllowedCorsOrigin",
                table: "Clients",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowedCorsOrigin",
                table: "Clients");
        }
    }
}
