using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class OptionalitemId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecurringWOs_Items_ItemId",
                table: "RecurringWOs");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "RecurringWOs",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "965a24a6-7006-4058-87f5-71aa311cd987");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "7383caaa-dc8a-4ceb-b094-97ba5566ce98");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "7eea6e3e-a542-4fc9-b644-1f926ceab221");

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringWOs_Items_ItemId",
                table: "RecurringWOs",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecurringWOs_Items_ItemId",
                table: "RecurringWOs");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "RecurringWOs",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "8de36a4a-5d2d-4822-b81e-cb01981ae9de");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "b59607a9-2499-48d6-adb6-0b5693ee227d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "464e08bc-eeae-4a06-bd23-6ee2c581a3f2");

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringWOs_Items_ItemId",
                table: "RecurringWOs",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
