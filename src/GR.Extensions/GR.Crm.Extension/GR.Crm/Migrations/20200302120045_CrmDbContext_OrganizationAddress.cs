using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_OrganizationAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Employees_EmployeeId",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_EmployeeId",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "CountryId",
                schema: "Crm",
                table: "OrganizationAddresses");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                schema: "Crm",
                table: "OrganizationAddresses");

            migrationBuilder.AlterColumn<string>(
                name: "FiscalCode",
                schema: "Crm",
                table: "Organizations",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Employees",
                schema: "Crm",
                table: "Organizations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GeoPosition",
                schema: "Crm",
                table: "Organizations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "CityId",
                schema: "Crm",
                table: "OrganizationAddresses",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Regions",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Changed = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    GeoPosition = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Changed = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    RegionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_Regions_RegionId",
                        column: x => x.RegionId,
                        principalSchema: "Crm",
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationAddresses_CityId",
                schema: "Crm",
                table: "OrganizationAddresses",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_RegionId",
                schema: "Crm",
                table: "Cities",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationAddresses_Cities_CityId",
                schema: "Crm",
                table: "OrganizationAddresses",
                column: "CityId",
                principalSchema: "Crm",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationAddresses_Cities_CityId",
                schema: "Crm",
                table: "OrganizationAddresses");

            migrationBuilder.DropTable(
                name: "Cities",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "Regions",
                schema: "Crm");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationAddresses_CityId",
                schema: "Crm",
                table: "OrganizationAddresses");

            migrationBuilder.DropColumn(
                name: "Employees",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "GeoPosition",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.AlterColumn<string>(
                name: "FiscalCode",
                schema: "Crm",
                table: "Organizations",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 13);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                schema: "Crm",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                schema: "Crm",
                table: "OrganizationAddresses",
                nullable: true,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryId",
                schema: "Crm",
                table: "OrganizationAddresses",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DistrictId",
                schema: "Crm",
                table: "OrganizationAddresses",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_EmployeeId",
                schema: "Crm",
                table: "Organizations",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Employees_EmployeeId",
                schema: "Crm",
                table: "Organizations",
                column: "EmployeeId",
                principalSchema: "Crm",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
