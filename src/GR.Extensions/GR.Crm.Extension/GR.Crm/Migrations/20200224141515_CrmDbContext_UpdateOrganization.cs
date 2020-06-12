using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_UpdateOrganization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CityId",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "CountryId",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Street",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Zip",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.CreateTable(
                name: "OrganizationAddresses",
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
                    OrganizationId = table.Column<Guid>(nullable: false),
                    CountryId = table.Column<string>(nullable: true),
                    CityId = table.Column<int>(nullable: true),
                    DistrictId = table.Column<Guid>(nullable: true),
                    Street = table.Column<string>(maxLength: 128, nullable: true),
                    Zip = table.Column<string>(maxLength: 28, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationAddresses_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalSchema: "Crm",
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkCategories",
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
                    Description = table.Column<string>(nullable: true)
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
                    Created = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Changed = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    OrganizationId = table.Column<Guid>(nullable: false),
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
                name: "IX_OrganizationAddresses_OrganizationId",
                schema: "Crm",
                table: "OrganizationAddresses",
                column: "OrganizationId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganizationAddresses",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "OrganizationWorkCategories",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "WorkCategories",
                schema: "Crm");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                schema: "Crm",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryId",
                schema: "Crm",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                schema: "Crm",
                table: "Organizations",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zip",
                schema: "Crm",
                table: "Organizations",
                maxLength: 28,
                nullable: true);
        }
    }
}
