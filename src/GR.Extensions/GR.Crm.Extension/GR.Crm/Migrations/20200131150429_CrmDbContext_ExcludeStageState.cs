using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_ExcludeStageState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leads_StageStates_StageStateId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropTable(
                name: "StageStates",
                schema: "Crm");

            migrationBuilder.DropColumn(
                name: "WorkFlowId",
                schema: "Crm",
                table: "Stages");

            migrationBuilder.RenameColumn(
                name: "StageStateId",
                schema: "Crm",
                table: "Leads",
                newName: "LeadStateId");

            migrationBuilder.RenameIndex(
                name: "IX_Leads_StageStateId",
                schema: "Crm",
                table: "Leads",
                newName: "IX_Leads_LeadStateId");

            migrationBuilder.CreateTable(
                name: "States",
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
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_States_LeadStateId",
                schema: "Crm",
                table: "Leads",
                column: "LeadStateId",
                principalSchema: "Crm",
                principalTable: "States",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leads_States_LeadStateId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropTable(
                name: "States",
                schema: "Crm");

            migrationBuilder.RenameColumn(
                name: "LeadStateId",
                schema: "Crm",
                table: "Leads",
                newName: "StageStateId");

            migrationBuilder.RenameIndex(
                name: "IX_Leads_LeadStateId",
                schema: "Crm",
                table: "Leads",
                newName: "IX_Leads_StageStateId");

            migrationBuilder.AddColumn<Guid>(
                name: "WorkFlowId",
                schema: "Crm",
                table: "Stages",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StageStates",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Changed = table.Column<DateTime>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    StageId = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    Version = table.Column<int>(nullable: false),
                    WorkFlowStateId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StageStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StageStates_Stages_StageId",
                        column: x => x.StageId,
                        principalSchema: "Crm",
                        principalTable: "Stages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StageStates_StageId",
                schema: "Crm",
                table: "StageStates",
                column: "StageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_StageStates_StageStateId",
                schema: "Crm",
                table: "Leads",
                column: "StageStateId",
                principalSchema: "Crm",
                principalTable: "StageStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
