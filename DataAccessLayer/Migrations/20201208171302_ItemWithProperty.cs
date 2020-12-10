using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class ItemWithProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecurringWOs_Issues_IssueId",
                table: "RecurringWOs");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_Issues_IssueId",
                table: "WorkOrders");

            migrationBuilder.AlterColumn<int>(
                name: "IssueId",
                table: "WorkOrders",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "CustomIssue",
                table: "WorkOrders",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IssueId",
                table: "RecurringWOs",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "CustomIssue",
                table: "RecurringWOs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PropertyId",
                table: "Items",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "PropertyId1",
                table: "Items",
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

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringWOs_Issues_IssueId",
                table: "RecurringWOs",
                column: "IssueId",
                principalTable: "Issues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrders_Issues_IssueId",
                table: "WorkOrders",
                column: "IssueId",
                principalTable: "Issues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Properties_PropertyId1",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_RecurringWOs_Issues_IssueId",
                table: "RecurringWOs");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_Issues_IssueId",
                table: "WorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_Items_PropertyId1",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CustomIssue",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "CustomIssue",
                table: "RecurringWOs");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "PropertyId1",
                table: "Items");

            migrationBuilder.AlterColumn<int>(
                name: "IssueId",
                table: "WorkOrders",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IssueId",
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
                value: "65d77501-74c8-4ac8-9f5f-d91835557a3c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "4739b2b8-9b06-4e06-ad03-76659ff4afde");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "2486654b-a128-43e0-a9c6-32f7997499ed");

            migrationBuilder.AddForeignKey(
                name: "FK_RecurringWOs_Issues_IssueId",
                table: "RecurringWOs",
                column: "IssueId",
                principalTable: "Issues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrders_Issues_IssueId",
                table: "WorkOrders",
                column: "IssueId",
                principalTable: "Issues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
