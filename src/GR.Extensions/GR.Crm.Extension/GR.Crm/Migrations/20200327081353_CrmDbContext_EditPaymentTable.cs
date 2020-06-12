using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_EditPaymentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrganizationName",
                schema: "Crm",
                table: "Payments",
                newName: "PaymentDestination");

            migrationBuilder.AddColumn<string>(
                name: "DocumentNumber",
                schema: "Crm",
                table: "Payments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentMappers",
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
                    PaymentId = table.Column<Guid>(nullable: true),
                    OrganizationId = table.Column<Guid>(nullable: true),
                    ProductId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMappers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentMappers_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalSchema: "Crm",
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentMappers_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalSchema: "Crm",
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentMappers_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Crm",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMappers_OrganizationId",
                schema: "Crm",
                table: "PaymentMappers",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMappers_PaymentId",
                schema: "Crm",
                table: "PaymentMappers",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMappers_ProductId",
                schema: "Crm",
                table: "PaymentMappers",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentMappers",
                schema: "Crm");

            migrationBuilder.DropColumn(
                name: "DocumentNumber",
                schema: "Crm",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "PaymentDestination",
                schema: "Crm",
                table: "Payments",
                newName: "OrganizationName");
        }
    }
}
