using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Platform.Migrations.AppIdentityMigrations
{
    /// <inheritdoc />
    public partial class AddResetCodeToUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "resetCode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "resetCode",
                table: "AspNetUsers");
        }
    }
}
