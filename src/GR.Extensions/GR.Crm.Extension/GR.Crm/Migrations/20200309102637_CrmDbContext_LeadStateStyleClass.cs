using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_LeadStateStyleClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StateStyleClass",
                schema: "Crm",
                table: "States",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Afiliat",
                schema: "Crm",
                table: "Organizations",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 128);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StateStyleClass",
                schema: "Crm",
                table: "States");

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
