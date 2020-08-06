using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class ForRecurring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRecurring",
                table: "WorkOrders");

            migrationBuilder.AddColumn<string>(
                name: "CronExpression",
                table: "WorkOrders",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Recurring",
                table: "WorkOrders",
                nullable: false,
                defaultValue: false);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CronExpression",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "Recurring",
                table: "WorkOrders");

            migrationBuilder.AddColumn<bool>(
                name: "IsRecurring",
                table: "WorkOrders",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
    }
}
