using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_AddPaymentCodeToPaymentMapped : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PaymentCodeId",
                schema: "Crm",
                table: "PaymentMappers",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMappers_PaymentCodeId",
                schema: "Crm",
                table: "PaymentMappers",
                column: "PaymentCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentMappers_PaymentCodes_PaymentCodeId",
                schema: "Crm",
                table: "PaymentMappers",
                column: "PaymentCodeId",
                principalSchema: "Crm",
                principalTable: "PaymentCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMappers_PaymentCodes_PaymentCodeId",
                schema: "Crm",
                table: "PaymentMappers");

            migrationBuilder.DropIndex(
                name: "IX_PaymentMappers_PaymentCodeId",
                schema: "Crm",
                table: "PaymentMappers");

            migrationBuilder.DropColumn(
                name: "PaymentCodeId",
                schema: "Crm",
                table: "PaymentMappers");
        }
    }
}
