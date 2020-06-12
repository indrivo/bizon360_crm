using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_addPLeadDDeadLine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OfferDeadLine",
                schema: "Crm",
                table: "Leads",
                newName: "StageDeadLine");

            migrationBuilder.RenameColumn(
                name: "ClarificationDeadLine",
                schema: "Crm",
                table: "Leads",
                newName: "DeadLine");

            migrationBuilder.AddColumn<int>(
                name: "Term",
                schema: "Crm",
                table: "Stages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Term",
                schema: "Crm",
                table: "Stages");

            migrationBuilder.RenameColumn(
                name: "StageDeadLine",
                schema: "Crm",
                table: "Leads",
                newName: "OfferDeadLine");

            migrationBuilder.RenameColumn(
                name: "DeadLine",
                schema: "Crm",
                table: "Leads",
                newName: "ClarificationDeadLine");
        }
    }
}
