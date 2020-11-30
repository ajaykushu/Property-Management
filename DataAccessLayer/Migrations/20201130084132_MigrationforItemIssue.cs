using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class MigrationforItemIssue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Issues",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "6688425e-0fb9-40c8-9585-097c874fd8ff");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "32aa2ce1-e5b2-44bd-a4be-a5b83c5893f5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "01fb0d12-f5fe-4bbd-a372-43a5a578a551");

            migrationBuilder.UpdateData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: 1,
                column: "ItemId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: 2,
                column: "ItemId",
                value: 2);

            migrationBuilder.CreateIndex(
                name: "IX_Issues_ItemId",
                table: "Issues",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Items_ItemId",
                table: "Issues",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Items_ItemId",
                table: "Issues");

            migrationBuilder.DropIndex(
                name: "IX_Issues_ItemId",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Issues");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "3f4a1a17-e53b-4d8f-bc81-db3d049a8faa");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "3d3f9549-4037-4c2b-90b4-6e38eca9918f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "34ddcc8c-adca-4749-a60f-2c501c989517");
        }
    }
}
