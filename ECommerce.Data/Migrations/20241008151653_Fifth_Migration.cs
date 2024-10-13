using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Data.Migrations
{
    public partial class Fifth_Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ValidityTill",
                table: "JwtTokens",
                newName: "RefreshTokenValidityTill");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RefreshTokenValidityTill",
                table: "JwtTokens",
                newName: "ValidityTill");
        }
    }
}
