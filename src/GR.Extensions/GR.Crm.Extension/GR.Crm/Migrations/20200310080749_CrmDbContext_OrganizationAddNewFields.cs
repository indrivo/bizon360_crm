using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_OrganizationAddNewFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Afiliat",
                schema: "Crm",
                table: "Organizations",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AddColumn<string>(
                name: "CodSwift",
                schema: "Crm",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodTva",
                schema: "Crm",
                table: "Organizations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodSwift",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "CodTva",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.AlterColumn<string>(
                name: "Afiliat",
                schema: "Crm",
                table: "Organizations",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128,
                oldNullable: true);
        }
    }
}
