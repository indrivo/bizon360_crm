using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContextt_addOrganizationEmployeeAndIndustry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                schema: "Crm",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IndustryId",
                schema: "Crm",
                table: "Organizations",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Employees",
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
                    Interval = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Industries",
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
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Industries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_EmployeeId",
                schema: "Crm",
                table: "Organizations",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_IndustryId",
                schema: "Crm",
                table: "Organizations",
                column: "IndustryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Employees_EmployeeId",
                schema: "Crm",
                table: "Organizations",
                column: "EmployeeId",
                principalSchema: "Crm",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Industries_IndustryId",
                schema: "Crm",
                table: "Organizations",
                column: "IndustryId",
                principalSchema: "Crm",
                principalTable: "Industries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Employees_EmployeeId",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Industries_IndustryId",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.DropTable(
                name: "Employees",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "Industries",
                schema: "Crm");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_EmployeeId",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_IndustryId",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "IndustryId",
                schema: "Crm",
                table: "Organizations");
        }
    }
}
