using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class CreateRecurringWo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EndAfterCount",
                table: "WorkOrders",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RecurringEndDate",
                table: "WorkOrders",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "1dcecae8-f201-4f8b-ac04-774d2932b794");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "94091e48-5fb1-4360-b967-eb53f91df6da");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "07df77c3-c42d-47cb-9d43-aa5a0e60dad4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndAfterCount",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "RecurringEndDate",
                table: "WorkOrders");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "87eb7a46-07bf-42d8-926c-50a2de00fb8e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "a1a93ca8-e063-45db-846e-bd4f1ef78a2b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "605a8015-2848-4b04-a182-b4be864aa20f");
        }
    }
}
