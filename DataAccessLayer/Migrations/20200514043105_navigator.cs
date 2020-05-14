using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class navigator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NotificationType",
                table: "Notifications",
                type: "varchar(2)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NavigatorId",
                table: "Notifications",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "2c2e3813-6e95-4f48-b714-d02ff0b71d33");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "85b55bf4-95fc-455a-af1b-c70a04e6232d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "e4c0235d-8e5d-4898-b40d-a11a66ee8963");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NavigatorId",
                table: "Notifications");

            migrationBuilder.AlterColumn<string>(
                name: "NotificationType",
                table: "Notifications",
                type: "varchar(1)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(2)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "a3ec4afd-fda6-4863-8d17-f9b696c587c4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "04082d3d-bf98-4891-94a0-10aa805efab0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "e5c4bd37-d6a8-4d38-b60a-11835622a225");
        }
    }
}
