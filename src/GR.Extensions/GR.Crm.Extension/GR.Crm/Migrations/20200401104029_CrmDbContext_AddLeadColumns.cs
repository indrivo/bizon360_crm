using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_AddLeadColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Commission",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "UnitsNumber",
                schema: "Crm",
                table: "Leads");
        }
    }
}
