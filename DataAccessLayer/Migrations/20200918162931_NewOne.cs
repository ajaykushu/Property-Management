using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class NewOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_WorkOrders_WorkOrderId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_WorkOrderId",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "RecurringWOs",
                nullable: false,
                defaultValueSql: "Concat('RWO', NEXT VALUE FOR workordersequence)",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "WorkOrderId",
                table: "Comments",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "2829255b-7b2a-4016-99cc-634b44a8fbe7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "2895e5e1-733e-4ab9-bda8-986522617bd2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "45b60997-aeaf-4080-bc6f-339f57586d62");

            migrationBuilder.InsertData(
                table: "Menu",
                columns: new[] { "Id", "CreatedByUserName", "CreatedTime", "MenuName", "UpdatedByUserName", "UpdatedTime" },
                values: new object[] { 19L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Recurring_WO", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Menu",
                keyColumn: "Id",
                keyValue: 19L);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "RecurringWOs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldDefaultValueSql: "Concat('RWO', NEXT VALUE FOR workordersequence)");

            migrationBuilder.AlterColumn<string>(
                name: "WorkOrderId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "76fe1008-2b3d-4dc0-8f06-cb5b87fd4b82");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "ee5ab939-3d26-4dc7-a643-7a35bdde27f4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "7d8f66f6-c391-4983-a387-85c9383b40ca");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_WorkOrderId",
                table: "Comments",
                column: "WorkOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_WorkOrders_WorkOrderId",
                table: "Comments",
                column: "WorkOrderId",
                principalTable: "WorkOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
