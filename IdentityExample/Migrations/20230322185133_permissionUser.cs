using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityExample.Migrations
{
    /// <inheritdoc />
    public partial class permissionUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserPermissions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserPermissions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
