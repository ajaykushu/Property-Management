using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class CascadeDel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_Items_ItemId",
                table: "WorkOrders");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "WorkOrders",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "13a620bc-0059-46b2-b57b-17eaba8b48cd");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "23dcf7b5-12a3-49fb-a4ef-2aaf0a2862a3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "389e9c13-2ca0-4ba8-b04f-dd347188b827");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrders_Items_ItemId",
                table: "WorkOrders",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_Items_ItemId",
                table: "WorkOrders");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "WorkOrders",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "f04d70b0-b2e1-4d90-a526-1268f9b13087");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "6da9cf3c-0395-4dc9-b055-a1f2cacf36f3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "24ee58ba-703d-469e-b124-33b6114ba249");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrders_Items_ItemId",
                table: "WorkOrders",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
