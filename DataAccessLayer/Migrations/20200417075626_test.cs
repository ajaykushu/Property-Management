using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Properties_PropertyId1",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_PropertyId1",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "PropertyId1",
                table: "Locations");

            migrationBuilder.AlterColumn<long>(
                name: "PropertyId",
                table: "Locations",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "7df7d67e-c1e2-41ff-b2f3-2e758efaa9a7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "ad44d1c0-3c61-46b6-89dc-5569b6879788");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "3644e5ec-0d48-41fe-8dc2-a3a3f90fb3d2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4L,
                column: "ConcurrencyStamp",
                value: "81aa425c-5b5d-487e-9947-f9417182a496");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_PropertyId",
                table: "Locations",
                column: "PropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Properties_PropertyId",
                table: "Locations",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Properties_PropertyId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_PropertyId",
                table: "Locations");

            migrationBuilder.AlterColumn<int>(
                name: "PropertyId",
                table: "Locations",
                type: "int",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "PropertyId1",
                table: "Locations",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "0fafff48-aef6-45bb-a512-3f000c127efc");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "3fcbfd28-15f2-4504-b85e-197f0e876e68");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "400e5bab-4a5c-4fd3-8e19-9eb1c35d330e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4L,
                column: "ConcurrencyStamp",
                value: "cd40cbac-f04e-48ab-ba7b-a6a349be5eb4");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_PropertyId1",
                table: "Locations",
                column: "PropertyId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Properties_PropertyId1",
                table: "Locations",
                column: "PropertyId1",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
