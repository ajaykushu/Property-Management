using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class Cron : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "2310cd3a-4c79-41c6-84fd-55062d8ba28f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "4c4af118-7ad6-4bac-8874-92cf0f30a29b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "98de7b73-8577-44ea-b319-8219482be3bf");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "cde25672-5b31-42d7-b520-2e31abb3894c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "a420a907-680a-40ba-85b1-7290e63e16ab");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "947d9898-5de7-49c5-95aa-e83edf7791e7");
        }
    }
}
