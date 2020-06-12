using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_AgreementName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BankAccount",
                schema: "Crm",
                table: "Products",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "Crm",
                table: "Agreements",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Crm",
                table: "Agreements");

            migrationBuilder.AlterColumn<string>(
                name: "BankAccount",
                schema: "Crm",
                table: "Products",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
