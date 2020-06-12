using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_OrganizationUpdateAddColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CityId",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "CountryId",
                schema: "Crm",
                table: "Organizations");
        }
    }
}
