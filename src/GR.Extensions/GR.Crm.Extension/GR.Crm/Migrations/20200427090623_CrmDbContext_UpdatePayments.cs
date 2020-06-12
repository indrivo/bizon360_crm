using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_UpdatePayments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMappers_Products_ProductId",
                schema: "Crm",
                table: "PaymentMappers");

            migrationBuilder.RenameColumn(
                name: "Vat",
                schema: "Crm",
                table: "Payments",
                newName: "UnitPriceWithoutTVA");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                schema: "Crm",
                table: "Payments",
                newName: "TotalTVA");

            migrationBuilder.RenameColumn(
                name: "Amount",
                schema: "Crm",
                table: "Payments",
                newName: "TotalPriceWithoutTVA");

            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                schema: "Crm",
                table: "Payments",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TVA",
                schema: "Crm",
                table: "Payments",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                schema: "Crm",
                table: "Payments",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                schema: "Crm",
                table: "PaymentMappers",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "WorkCategoryId",
                schema: "Crm",
                table: "PaymentMappers",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "FiscalCode",
                schema: "Crm",
                table: "Organizations",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 13);

            migrationBuilder.CreateTable(
                name: "PaymentCodes",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Changed = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    WorkCategoryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentCodes_WorkCategories_WorkCategoryId",
                        column: x => x.WorkCategoryId,
                        principalSchema: "Crm",
                        principalTable: "WorkCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMappers_WorkCategoryId",
                schema: "Crm",
                table: "PaymentMappers",
                column: "WorkCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCodes_WorkCategoryId",
                schema: "Crm",
                table: "PaymentCodes",
                column: "WorkCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentMappers_Products_ProductId",
                schema: "Crm",
                table: "PaymentMappers",
                column: "ProductId",
                principalSchema: "Crm",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentMappers_WorkCategories_WorkCategoryId",
                schema: "Crm",
                table: "PaymentMappers",
                column: "WorkCategoryId",
                principalSchema: "Crm",
                principalTable: "WorkCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMappers_Products_ProductId",
                schema: "Crm",
                table: "PaymentMappers");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMappers_WorkCategories_WorkCategoryId",
                schema: "Crm",
                table: "PaymentMappers");

            migrationBuilder.DropTable(
                name: "PaymentCodes",
                schema: "Crm");

            migrationBuilder.DropIndex(
                name: "IX_PaymentMappers_WorkCategoryId",
                schema: "Crm",
                table: "PaymentMappers");

            migrationBuilder.DropColumn(
                name: "Quantity",
                schema: "Crm",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "TVA",
                schema: "Crm",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                schema: "Crm",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "WorkCategoryId",
                schema: "Crm",
                table: "PaymentMappers");

            migrationBuilder.RenameColumn(
                name: "UnitPriceWithoutTVA",
                schema: "Crm",
                table: "Payments",
                newName: "Vat");

            migrationBuilder.RenameColumn(
                name: "TotalTVA",
                schema: "Crm",
                table: "Payments",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "TotalPriceWithoutTVA",
                schema: "Crm",
                table: "Payments",
                newName: "Amount");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                schema: "Crm",
                table: "PaymentMappers",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FiscalCode",
                schema: "Crm",
                table: "Organizations",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 15);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentMappers_Products_ProductId",
                schema: "Crm",
                table: "PaymentMappers",
                column: "ProductId",
                principalSchema: "Crm",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
