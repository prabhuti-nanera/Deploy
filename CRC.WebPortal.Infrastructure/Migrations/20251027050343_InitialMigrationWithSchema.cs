using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRC.WebPortal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrationWithSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "crc_webportal");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Users",
                newSchema: "crc_webportal");

            migrationBuilder.RenameTable(
                name: "Units",
                newName: "Units",
                newSchema: "crc_webportal");

            migrationBuilder.RenameTable(
                name: "Projects",
                newName: "Projects",
                newSchema: "crc_webportal");

            migrationBuilder.RenameTable(
                name: "Buildings",
                newName: "Buildings",
                newSchema: "crc_webportal");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Users",
                schema: "crc_webportal",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "Units",
                schema: "crc_webportal",
                newName: "Units");

            migrationBuilder.RenameTable(
                name: "Projects",
                schema: "crc_webportal",
                newName: "Projects");

            migrationBuilder.RenameTable(
                name: "Buildings",
                schema: "crc_webportal",
                newName: "Buildings");
        }
    }
}
