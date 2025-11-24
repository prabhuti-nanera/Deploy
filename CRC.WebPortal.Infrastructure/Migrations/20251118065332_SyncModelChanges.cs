using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CRC.WebPortal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SyncModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "crc_webportal",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                schema: "crc_webportal",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "crc_webportal",
                table: "Units",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "crc_webportal",
                table: "Units",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "crc_webportal",
                table: "Units",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OwnershipStatus",
                schema: "crc_webportal",
                table: "Units",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "crc_webportal",
                table: "Units",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitTypeId",
                schema: "crc_webportal",
                table: "Units",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                schema: "crc_webportal",
                table: "Units",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "crc_webportal",
                table: "Projects",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "crc_webportal",
                table: "Projects",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "crc_webportal",
                table: "Projects",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "crc_webportal",
                table: "Projects",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                schema: "crc_webportal",
                table: "Projects",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "crc_webportal",
                table: "Buildings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "crc_webportal",
                table: "Buildings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "crc_webportal",
                table: "Buildings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "crc_webportal",
                table: "Buildings",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                schema: "crc_webportal",
                table: "Buildings",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NumberingPatterns",
                schema: "crc_webportal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartFloorNumber = table.Column<int>(type: "integer", nullable: false),
                    UnitNameDigits = table.Column<int>(type: "integer", nullable: false),
                    ApplyTo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumberingPatterns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NumberingPatterns_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "crc_webportal",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UnitOwnerships",
                schema: "crc_webportal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OwnerEmail = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OwnerMobile = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SellingDetails = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    SalePrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    SaleDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SaleStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UnitId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitOwnerships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitOwnerships_Units_UnitId",
                        column: x => x.UnitId,
                        principalSchema: "crc_webportal",
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UnitTypes",
                schema: "crc_webportal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitTypes_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "crc_webportal",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NumberingPatternRows",
                schema: "crc_webportal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UnitType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FirstDigit = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SecondDigit = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ThirdDigit = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CustomFirstDigit = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CustomSecondDigit = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CustomThirdDigit = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DigitsJson = table.Column<string>(type: "jsonb", nullable: false),
                    CustomDigitsJson = table.Column<string>(type: "jsonb", nullable: false),
                    OriginalDigitCount = table.Column<int>(type: "integer", nullable: false),
                    Result = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NumberingPatternId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumberingPatternRows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NumberingPatternRows_NumberingPatterns_NumberingPatternId",
                        column: x => x.NumberingPatternId,
                        principalSchema: "crc_webportal",
                        principalTable: "NumberingPatterns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Units_UnitTypeId",
                schema: "crc_webportal",
                table: "Units",
                column: "UnitTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NumberingPatternRows_NumberingPatternId",
                schema: "crc_webportal",
                table: "NumberingPatternRows",
                column: "NumberingPatternId");

            migrationBuilder.CreateIndex(
                name: "IX_NumberingPatterns_ProjectId",
                schema: "crc_webportal",
                table: "NumberingPatterns",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitOwnerships_OwnerEmail",
                schema: "crc_webportal",
                table: "UnitOwnerships",
                column: "OwnerEmail");

            migrationBuilder.CreateIndex(
                name: "IX_UnitOwnerships_UnitId",
                schema: "crc_webportal",
                table: "UnitOwnerships",
                column: "UnitId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UnitTypes_ProjectId_Name",
                schema: "crc_webportal",
                table: "UnitTypes",
                columns: new[] { "ProjectId", "Name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_UnitTypes_UnitTypeId",
                schema: "crc_webportal",
                table: "Units",
                column: "UnitTypeId",
                principalSchema: "crc_webportal",
                principalTable: "UnitTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_UnitTypes_UnitTypeId",
                schema: "crc_webportal",
                table: "Units");

            migrationBuilder.DropTable(
                name: "NumberingPatternRows",
                schema: "crc_webportal");

            migrationBuilder.DropTable(
                name: "UnitOwnerships",
                schema: "crc_webportal");

            migrationBuilder.DropTable(
                name: "UnitTypes",
                schema: "crc_webportal");

            migrationBuilder.DropTable(
                name: "NumberingPatterns",
                schema: "crc_webportal");

            migrationBuilder.DropIndex(
                name: "IX_Units_UnitTypeId",
                schema: "crc_webportal",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "crc_webportal",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "crc_webportal",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "crc_webportal",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "crc_webportal",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "crc_webportal",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "OwnershipStatus",
                schema: "crc_webportal",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "crc_webportal",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "UnitTypeId",
                schema: "crc_webportal",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "crc_webportal",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "crc_webportal",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "crc_webportal",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "crc_webportal",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "crc_webportal",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "crc_webportal",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "crc_webportal",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "crc_webportal",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "crc_webportal",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "crc_webportal",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "crc_webportal",
                table: "Buildings");
        }
    }
}
