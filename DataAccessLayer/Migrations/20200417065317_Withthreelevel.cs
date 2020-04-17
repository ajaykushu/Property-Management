using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class Withthreelevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PropertyId",
                table: "Locations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "PropertyId1",
                table: "Locations",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Properties_PropertyId1",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_PropertyId1",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "PropertyId1",
                table: "Locations");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "b3faa393-c744-4b14-87d9-91113898e82e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "8c0f19b2-4fc6-48f8-96fc-84ae1d254a83");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "95c7a5f2-34c7-4d26-b94c-8035a203b44b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4L,
                column: "ConcurrencyStamp",
                value: "737bc7f1-f568-4adf-945f-122c9179c7eb");
        }
    }
}
