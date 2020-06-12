using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContextt_addOrganizationDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeesNumber",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Crm",
                table: "Organizations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.AddColumn<int>(
                name: "EmployeesNumber",
                schema: "Crm",
                table: "Organizations",
                nullable: true);
        }
    }
}
