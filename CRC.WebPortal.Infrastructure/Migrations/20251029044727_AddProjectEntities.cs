using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRC.WebPortal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Units_BuildingId",
                schema: "crc_webportal",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Buildings_ProjectId",
                schema: "crc_webportal",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "IsSpecial",
                schema: "crc_webportal",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "SpecialType",
                schema: "crc_webportal",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "AdminContact",
                schema: "crc_webportal",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectType",
                schema: "crc_webportal",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "GroupName",
                schema: "crc_webportal",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "StartingNumber",
                schema: "crc_webportal",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "TotalUnits",
                schema: "crc_webportal",
                table: "Buildings");

            migrationBuilder.AlterColumn<string>(
                name: "UnitNumber",
                schema: "crc_webportal",
                table: "Units",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "Position",
                schema: "crc_webportal",
                table: "Units",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "crc_webportal",
                table: "Projects",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "AdminName",
                schema: "crc_webportal",
                table: "Projects",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                schema: "crc_webportal",
                table: "Projects",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "AdminMobile",
                schema: "crc_webportal",
                table: "Projects",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                schema: "crc_webportal",
                table: "Projects",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                schema: "crc_webportal",
                table: "Buildings",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "crc_webportal",
                table: "Buildings",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_Units_BuildingId_UnitNumber",
                schema: "crc_webportal",
                table: "Units",
                columns: new[] { "BuildingId", "UnitNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_ProjectId_Name",
                schema: "crc_webportal",
                table: "Buildings",
                columns: new[] { "ProjectId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Units_BuildingId_UnitNumber",
                schema: "crc_webportal",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Buildings_ProjectId_Name",
                schema: "crc_webportal",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "Position",
                schema: "crc_webportal",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "AdminMobile",
                schema: "crc_webportal",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Type",
                schema: "crc_webportal",
                table: "Projects");

            migrationBuilder.AlterColumn<string>(
                name: "UnitNumber",
                schema: "crc_webportal",
                table: "Units",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<bool>(
                name: "IsSpecial",
                schema: "crc_webportal",
                table: "Units",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SpecialType",
                schema: "crc_webportal",
                table: "Units",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "crc_webportal",
                table: "Projects",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "AdminName",
                schema: "crc_webportal",
                table: "Projects",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                schema: "crc_webportal",
                table: "Projects",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "AdminContact",
                schema: "crc_webportal",
                table: "Projects",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjectType",
                schema: "crc_webportal",
                table: "Projects",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                schema: "crc_webportal",
                table: "Buildings",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "crc_webportal",
                table: "Buildings",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "GroupName",
                schema: "crc_webportal",
                table: "Buildings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StartingNumber",
                schema: "crc_webportal",
                table: "Buildings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalUnits",
                schema: "crc_webportal",
                table: "Buildings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Units_BuildingId",
                schema: "crc_webportal",
                table: "Units",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_ProjectId",
                schema: "crc_webportal",
                table: "Buildings",
                column: "ProjectId");
        }
    }
}
