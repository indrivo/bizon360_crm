using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_OrganizationAddressCity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationAddresses_Cities_CityId",
                schema: "Crm",
                table: "OrganizationAddresses");

            migrationBuilder.AlterColumn<Guid>(
                name: "CityId",
                schema: "Crm",
                table: "OrganizationAddresses",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationAddresses_Cities_CityId",
                schema: "Crm",
                table: "OrganizationAddresses",
                column: "CityId",
                principalSchema: "Crm",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationAddresses_Cities_CityId",
                schema: "Crm",
                table: "OrganizationAddresses");

            migrationBuilder.AlterColumn<Guid>(
                name: "CityId",
                schema: "Crm",
                table: "OrganizationAddresses",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationAddresses_Cities_CityId",
                schema: "Crm",
                table: "OrganizationAddresses",
                column: "CityId",
                principalSchema: "Crm",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
