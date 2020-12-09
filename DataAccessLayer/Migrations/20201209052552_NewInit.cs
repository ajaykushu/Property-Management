using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class NewInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Properties_PropertyId1",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_PropertyId1",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "PropertyId1",
                table: "Items");

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Items",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "5fc4bc8c-a36d-4c3b-b69a-2878b84bf864");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "0a3d9216-e334-4df8-804d-5cdf9eb9c55d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "7a8efbb1-fa24-430a-9727-9bd80552888f");

            migrationBuilder.CreateIndex(
                name: "IX_Items_LocationId",
                table: "Items",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Locations_LocationId",
                table: "Items",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Locations_LocationId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_LocationId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Items");

            migrationBuilder.AddColumn<int>(
                name: "PropertyId",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "PropertyId1",
                table: "Items",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "8f80acf5-78b1-43b9-9002-7d4528574a44");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "d90b930d-cf7c-4deb-a09c-4b398dec0072");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "302d2f9f-bda8-43de-9a9a-c0b909ef6016");

            migrationBuilder.CreateIndex(
                name: "IX_Items_PropertyId1",
                table: "Items",
                column: "PropertyId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Properties_PropertyId1",
                table: "Items",
                column: "PropertyId1",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
