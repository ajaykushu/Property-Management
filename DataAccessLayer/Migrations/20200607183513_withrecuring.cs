using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class withrecuring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignedToDeptId",
                table: "WorkOrders",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RecurringTime",
                table: "WorkOrders",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RecurringType",
                table: "WorkOrders",
                type: "varchar(1)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "24fb3a8e-bc02-4045-ac3a-53f76cff30c7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "0c16ae40-7436-40d8-9721-e6a730b1b03e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "9fdfa6fd-4eee-4e76-9596-47aac7b6dbd3");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_AssignedToDeptId",
                table: "WorkOrders",
                column: "AssignedToDeptId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrders_Departments_AssignedToDeptId",
                table: "WorkOrders",
                column: "AssignedToDeptId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_Departments_AssignedToDeptId",
                table: "WorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_AssignedToDeptId",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "AssignedToDeptId",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "RecurringTime",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "RecurringType",
                table: "WorkOrders");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "1c4bf0e0-01e5-418c-8886-e07eb77f8cd6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "cf644c7d-d97e-4e97-bdbc-a85fbec87280");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "08dc6dc4-0001-40b2-aba6-b8fff4322568");
        }
    }
}
