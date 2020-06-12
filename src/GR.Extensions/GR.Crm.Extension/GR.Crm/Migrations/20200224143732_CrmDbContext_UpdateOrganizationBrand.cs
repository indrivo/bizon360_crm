using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_UpdateOrganizationBrand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Afiliat",
                schema: "Crm",
                table: "Organizations",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                schema: "Crm",
                table: "Organizations",
                maxLength: 128,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Afiliat",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Brand",
                schema: "Crm",
                table: "Organizations");
        }
    }
}
