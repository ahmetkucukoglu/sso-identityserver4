using Microsoft.EntityFrameworkCore.Migrations;

namespace SSO.Domain.Migrations
{
    public partial class AddedRequireClientSecretToClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RequireClientSecret",
                table: "Clients",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequireClientSecret",
                table: "Clients");
        }
    }
}
