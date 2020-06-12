using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_addProductToLead : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                schema: "Crm",
                table: "Leads",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationAddressId",
                schema: "Crm",
                table: "Agreements",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Leads_ProductId",
                schema: "Crm",
                table: "Leads",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_OrganizationAddressId",
                schema: "Crm",
                table: "Agreements",
                column: "OrganizationAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_OrganizationAddresses_OrganizationAddressId",
                schema: "Crm",
                table: "Agreements",
                column: "OrganizationAddressId",
                principalSchema: "Crm",
                principalTable: "OrganizationAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_Products_ProductId",
                schema: "Crm",
                table: "Leads",
                column: "ProductId",
                principalSchema: "Crm",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_OrganizationAddresses_OrganizationAddressId",
                schema: "Crm",
                table: "Agreements");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_Products_ProductId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Leads_ProductId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Agreements_OrganizationAddressId",
                schema: "Crm",
                table: "Agreements");

            migrationBuilder.DropColumn(
                name: "ProductId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "OrganizationAddressId",
                schema: "Crm",
                table: "Agreements");
        }
    }
}
