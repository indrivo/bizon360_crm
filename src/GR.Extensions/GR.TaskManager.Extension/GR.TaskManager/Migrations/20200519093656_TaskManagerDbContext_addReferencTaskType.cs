using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.TaskManager.Migrations
{
    public partial class TaskManagerDbContext_addReferencTaskType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TaskTypeId",
                schema: "Task",
                table: "Tasks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskTypeId",
                schema: "Task",
                table: "Tasks",
                column: "TaskTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskTypes_TaskTypeId",
                schema: "Task",
                table: "Tasks",
                column: "TaskTypeId",
                principalSchema: "Task",
                principalTable: "TaskTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskTypes_TaskTypeId",
                schema: "Task",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_TaskTypeId",
                schema: "Task",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TaskTypeId",
                schema: "Task",
                table: "Tasks");
        }
    }
}
