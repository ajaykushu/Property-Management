using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class addingRecurrng : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParentWorkOrderId",
                table: "WorkOrders",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "f4328861-3dfd-4586-a84e-f730890ad7b0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "5cff61fb-0cc6-4346-bcdd-c045dffbf683");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "d3540943-1c0f-484e-9139-59bb46ec5289");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_ParentWorkOrderId",
                table: "WorkOrders",
                column: "ParentWorkOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrders_WorkOrders_ParentWorkOrderId",
                table: "WorkOrders",
                column: "ParentWorkOrderId",
                principalTable: "WorkOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_WorkOrders_ParentWorkOrderId",
                table: "WorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_ParentWorkOrderId",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "ParentWorkOrderId",
                table: "WorkOrders");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "a81c8f7a-5cec-4827-a746-b23371a6fcc1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "c2df9448-938f-4719-b220-334c2922674b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "eff581ad-97b5-4df2-a404-14e9662f5c11");
        }
    }
}
