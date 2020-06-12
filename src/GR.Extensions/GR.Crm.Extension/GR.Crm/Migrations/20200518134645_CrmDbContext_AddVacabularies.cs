using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_AddVacabularies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Commission",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "UnitsNumber",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.AddColumn<DateTime>(
                name: "ClarificationDeadline",
                schema: "Crm",
                table: "Leads",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ContactId",
                schema: "Crm",
                table: "Leads",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductTypeId",
                schema: "Crm",
                table: "Leads",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceTypeId",
                schema: "Crm",
                table: "Leads",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SolutionTypeId",
                schema: "Crm",
                table: "Leads",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SourceId",
                schema: "Crm",
                table: "Leads",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TechnologyTypeId",
                schema: "Crm",
                table: "Leads",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductTypes",
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
                    table.PrimaryKey("PK_ProductTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTypes",
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
                    table.PrimaryKey("PK_ServiceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SolutionTypes",
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
                    table.PrimaryKey("PK_SolutionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sources",
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
                    table.PrimaryKey("PK_Sources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TechnologyTypes",
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
                    table.PrimaryKey("PK_TechnologyTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leads_ContactId",
                schema: "Crm",
                table: "Leads",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_ProductTypeId",
                schema: "Crm",
                table: "Leads",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_ServiceTypeId",
                schema: "Crm",
                table: "Leads",
                column: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_SolutionTypeId",
                schema: "Crm",
                table: "Leads",
                column: "SolutionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_SourceId",
                schema: "Crm",
                table: "Leads",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_TechnologyTypeId",
                schema: "Crm",
                table: "Leads",
                column: "TechnologyTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_Contacts_ContactId",
                schema: "Crm",
                table: "Leads",
                column: "ContactId",
                principalSchema: "Crm",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_ProductTypes_ProductTypeId",
                schema: "Crm",
                table: "Leads",
                column: "ProductTypeId",
                principalSchema: "Crm",
                principalTable: "ProductTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_ServiceTypes_ServiceTypeId",
                schema: "Crm",
                table: "Leads",
                column: "ServiceTypeId",
                principalSchema: "Crm",
                principalTable: "ServiceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_SolutionTypes_SolutionTypeId",
                schema: "Crm",
                table: "Leads",
                column: "SolutionTypeId",
                principalSchema: "Crm",
                principalTable: "SolutionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_Sources_SourceId",
                schema: "Crm",
                table: "Leads",
                column: "SourceId",
                principalSchema: "Crm",
                principalTable: "Sources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_TechnologyTypes_TechnologyTypeId",
                schema: "Crm",
                table: "Leads",
                column: "TechnologyTypeId",
                principalSchema: "Crm",
                principalTable: "TechnologyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leads_Contacts_ContactId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_ProductTypes_ProductTypeId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_ServiceTypes_ServiceTypeId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_SolutionTypes_SolutionTypeId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_Sources_SourceId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_TechnologyTypes_TechnologyTypeId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropTable(
                name: "ProductTypes",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "ServiceTypes",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "SolutionTypes",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "Sources",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "TechnologyTypes",
                schema: "Crm");

            migrationBuilder.DropIndex(
                name: "IX_Leads_ContactId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Leads_ProductTypeId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Leads_ServiceTypeId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Leads_SolutionTypeId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Leads_SourceId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Leads_TechnologyTypeId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "ClarificationDeadline",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "ContactId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "ProductTypeId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "ServiceTypeId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "SolutionTypeId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "SourceId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "TechnologyTypeId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.AddColumn<decimal>(
                name: "Commission",
                schema: "Crm",
                table: "Leads",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "UnitsNumber",
                schema: "Crm",
                table: "Leads",
                nullable: false,
                defaultValue: 0);
        }
    }
}
