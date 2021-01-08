using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class EffortDbUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaxBO",
                table: "Efforts");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Efforts",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "caecf252-6c03-4d44-9288-ead6939485b1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "c61f3a11-ca2b-40e4-9988-16edceec5fbf");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "0ec7db1d-55a7-4a7d-99bb-828fd480a180");

            migrationBuilder.CreateIndex(
                name: "IX_Efforts_UserId",
                table: "Efforts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Efforts_AspNetUsers_UserId",
                table: "Efforts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Efforts_AspNetUsers_UserId",
                table: "Efforts");

            migrationBuilder.DropIndex(
                name: "IX_Efforts_UserId",
                table: "Efforts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Efforts");

            migrationBuilder.AddColumn<int>(
                name: "TaxBO",
                table: "Efforts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "830b0b85-45cb-4d21-912a-e1f6ff0e2c98");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "4b237691-4707-4ff0-a36b-b44c86e379f0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "32897b4e-ebcf-4790-93f8-c24165ec8f63");
        }
    }
}
