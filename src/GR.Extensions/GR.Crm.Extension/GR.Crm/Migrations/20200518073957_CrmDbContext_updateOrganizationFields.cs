using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_updateOrganizationFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leads_WorkCategories_WorkCategoryId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentCodes_WorkCategories_WorkCategoryId",
                schema: "Crm",
                table: "PaymentCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMappers_WorkCategories_WorkCategoryId",
                schema: "Crm",
                table: "PaymentMappers");

            migrationBuilder.DropTable(
                name: "OrganizationWorkCategories",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "WorkCategories",
                schema: "Crm");

            migrationBuilder.DropIndex(
                name: "IX_PaymentMappers_WorkCategoryId",
                schema: "Crm",
                table: "PaymentMappers");

            migrationBuilder.DropIndex(
                name: "IX_PaymentCodes_WorkCategoryId",
                schema: "Crm",
                table: "PaymentCodes");

            migrationBuilder.DropIndex(
                name: "IX_Leads_WorkCategoryId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "GeoPosition",
                schema: "Crm",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "WorkCategoryId",
                schema: "Crm",
                table: "PaymentMappers");

            migrationBuilder.DropColumn(
                name: "WorkCategoryId",
                schema: "Crm",
                table: "PaymentCodes");

            migrationBuilder.DropColumn(
                name: "Afiliat",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Employees",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "GeoPosition",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "WorkCategoryId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.RenameColumn(
                name: "CodTva",
                schema: "Crm",
                table: "Organizations",
                newName: "VitCode");

            migrationBuilder.AlterColumn<string>(
                name: "FiscalCode",
                schema: "Crm",
                table: "Organizations",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 15);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                schema: "Crm",
                table: "Organizations",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "VitCode",
                schema: "Crm",
                table: "Organizations",
                newName: "CodTva");

            migrationBuilder.AddColumn<int>(
                name: "GeoPosition",
                schema: "Crm",
                table: "Regions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "WorkCategoryId",
                schema: "Crm",
                table: "PaymentMappers",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WorkCategoryId",
                schema: "Crm",
                table: "PaymentCodes",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "FiscalCode",
                schema: "Crm",
                table: "Organizations",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Afiliat",
                schema: "Crm",
                table: "Organizations",
                maxLength: 128,
                nullable: true);

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

            migrationBuilder.AddColumn<Guid>(
                name: "WorkCategoryId",
                schema: "Crm",
                table: "Leads",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WorkCategories",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Changed = table.Column<DateTime>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    Version = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationWorkCategories",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Changed = table.Column<DateTime>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    Version = table.Column<int>(nullable: false),
                    WorkCategoryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationWorkCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationWorkCategories_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalSchema: "Crm",
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationWorkCategories_WorkCategories_WorkCategoryId",
                        column: x => x.WorkCategoryId,
                        principalSchema: "Crm",
                        principalTable: "WorkCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMappers_WorkCategoryId",
                schema: "Crm",
                table: "PaymentMappers",
                column: "WorkCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCodes_WorkCategoryId",
                schema: "Crm",
                table: "PaymentCodes",
                column: "WorkCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_WorkCategoryId",
                schema: "Crm",
                table: "Leads",
                column: "WorkCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationWorkCategories_OrganizationId",
                schema: "Crm",
                table: "OrganizationWorkCategories",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationWorkCategories_WorkCategoryId",
                schema: "Crm",
                table: "OrganizationWorkCategories",
                column: "WorkCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_WorkCategories_WorkCategoryId",
                schema: "Crm",
                table: "Leads",
                column: "WorkCategoryId",
                principalSchema: "Crm",
                principalTable: "WorkCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentCodes_WorkCategories_WorkCategoryId",
                schema: "Crm",
                table: "PaymentCodes",
                column: "WorkCategoryId",
                principalSchema: "Crm",
                principalTable: "WorkCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentMappers_WorkCategories_WorkCategoryId",
                schema: "Crm",
                table: "PaymentMappers",
                column: "WorkCategoryId",
                principalSchema: "Crm",
                principalTable: "WorkCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
