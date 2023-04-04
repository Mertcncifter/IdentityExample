using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityExample.Migrations
{
    /// <inheritdoc />
    public partial class permissionEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "Permission",
                table: "UserPermissions",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Permission",
                table: "UserPermissions",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");
        }
    }
}
