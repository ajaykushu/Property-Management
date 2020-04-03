using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class newmig46 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_AspNetUsers_RequestedById",
                table: "WorkOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_AspNetUsers_RequestedById1",
                table: "WorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_RequestedById",
                table: "WorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_RequestedById1",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "RequestedById",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "RequestedById1",
                table: "WorkOrders");

            migrationBuilder.AddColumn<long>(
                name: "AssignedToId",
                table: "WorkOrders",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AssignedToId1",
                table: "WorkOrders",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AssignedToRoleId",
                table: "WorkOrders",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AssignedToRoleId1",
                table: "WorkOrders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestedBy",
                table: "WorkOrders",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "dee3bb28-c417-4e4d-9bcd-c41540582ddc");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "bf525e46-af21-44cc-92b6-0c356fa0de31");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_AssignedToId",
                table: "WorkOrders",
                column: "AssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_AssignedToId1",
                table: "WorkOrders",
                column: "AssignedToId1");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_AssignedToRoleId",
                table: "WorkOrders",
                column: "AssignedToRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_AssignedToRoleId1",
                table: "WorkOrders",
                column: "AssignedToRoleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrders_AspNetUsers_AssignedToId",
                table: "WorkOrders",
                column: "AssignedToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrders_AspNetUsers_AssignedToId1",
                table: "WorkOrders",
                column: "AssignedToId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrders_AspNetRoles_AssignedToRoleId",
                table: "WorkOrders",
                column: "AssignedToRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrders_AspNetRoles_AssignedToRoleId1",
                table: "WorkOrders",
                column: "AssignedToRoleId1",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_AspNetUsers_AssignedToId",
                table: "WorkOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_AspNetUsers_AssignedToId1",
                table: "WorkOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_AspNetRoles_AssignedToRoleId",
                table: "WorkOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_AspNetRoles_AssignedToRoleId1",
                table: "WorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_AssignedToId",
                table: "WorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_AssignedToId1",
                table: "WorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_AssignedToRoleId",
                table: "WorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_AssignedToRoleId1",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "AssignedToId",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "AssignedToId1",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "AssignedToRoleId",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "AssignedToRoleId1",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "RequestedBy",
                table: "WorkOrders");

            migrationBuilder.AddColumn<long>(
                name: "RequestedById",
                table: "WorkOrders",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RequestedById1",
                table: "WorkOrders",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "58e7cdc1-43d1-49eb-b45a-d71df2747400");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "177de418-d1c1-451b-a5da-82f7801be21a");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_RequestedById",
                table: "WorkOrders",
                column: "RequestedById");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_RequestedById1",
                table: "WorkOrders",
                column: "RequestedById1");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrders_AspNetUsers_RequestedById",
                table: "WorkOrders",
                column: "RequestedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrders_AspNetUsers_RequestedById1",
                table: "WorkOrders",
                column: "RequestedById1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}