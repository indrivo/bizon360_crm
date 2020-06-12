using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_WorkCategoryToLead : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WorkCategoryId",
                schema: "Crm",
                table: "Leads",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Leads_WorkCategoryId",
                schema: "Crm",
                table: "Leads",
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leads_WorkCategories_WorkCategoryId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Leads_WorkCategoryId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "WorkCategoryId",
                schema: "Crm",
                table: "Leads");
        }
    }
}
