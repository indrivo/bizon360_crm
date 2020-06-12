using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_AddTeamToLead : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TeamId",
                schema: "Crm",
                table: "Leads",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Leads_TeamId",
                schema: "Crm",
                table: "Leads",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_Teams_TeamId",
                schema: "Crm",
                table: "Leads",
                column: "TeamId",
                principalSchema: "Crm",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leads_Teams_TeamId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Leads_TeamId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "TeamId",
                schema: "Crm",
                table: "Leads");
        }
    }
}
