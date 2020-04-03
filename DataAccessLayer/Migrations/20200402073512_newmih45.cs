using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class newmih45 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_AspNetUsers_ApplicationUserId",
                table: "WorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_ApplicationUserId",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "WorkOrders");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserName",
                table: "WorkOrders",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "WorkOrders",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "RequestedById",
                table: "WorkOrders",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RequestedById1",
                table: "WorkOrders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserName",
                table: "WorkOrders",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "WorkOrders",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserName",
                table: "UserProperties",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "UserProperties",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserName",
                table: "UserProperties",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "UserProperties",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserName",
                table: "Stages",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "Stages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserName",
                table: "Stages",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "Stages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserName",
                table: "RoleMenuMaps",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "RoleMenuMaps",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserName",
                table: "RoleMenuMaps",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "RoleMenuMaps",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserName",
                table: "PropertyType",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "PropertyType",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserName",
                table: "PropertyType",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "PropertyType",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserName",
                table: "Properties",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "Properties",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserName",
                table: "Properties",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "Properties",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserName",
                table: "Menu",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "Menu",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserName",
                table: "Menu",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "Menu",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserName",
                table: "Languages",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "Languages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserName",
                table: "Languages",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "Languages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserName",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "Items",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserName",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "Items",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserName",
                table: "Issues",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "Issues",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserName",
                table: "Issues",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "Issues",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserName",
                table: "Department",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "Department",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserName",
                table: "Department",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "Department",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserName",
                table: "Comments",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "Comments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserName",
                table: "Comments",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "Comments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "CreatedByUserName",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "RequestedById",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "RequestedById1",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserName",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "CreatedByUserName",
                table: "UserProperties");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "UserProperties");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserName",
                table: "UserProperties");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "UserProperties");

            migrationBuilder.DropColumn(
                name: "CreatedByUserName",
                table: "Stages");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "Stages");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserName",
                table: "Stages");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "Stages");

            migrationBuilder.DropColumn(
                name: "CreatedByUserName",
                table: "RoleMenuMaps");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "RoleMenuMaps");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserName",
                table: "RoleMenuMaps");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "RoleMenuMaps");

            migrationBuilder.DropColumn(
                name: "CreatedByUserName",
                table: "PropertyType");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "PropertyType");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserName",
                table: "PropertyType");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "PropertyType");

            migrationBuilder.DropColumn(
                name: "CreatedByUserName",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserName",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "CreatedByUserName",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserName",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "CreatedByUserName",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserName",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "CreatedByUserName",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserName",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CreatedByUserName",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserName",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "CreatedByUserName",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserName",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "CreatedByUserName",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserName",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "Comments");

            migrationBuilder.AddColumn<long>(
                name: "ApplicationUserId",
                table: "WorkOrders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "WorkOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                table: "WorkOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "2e2b589a-8cce-4cfc-ae21-06c76c5a8385");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "605fa7c4-8d21-4d85-8548-a432e7ccade7");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_ApplicationUserId",
                table: "WorkOrders",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrders_AspNetUsers_ApplicationUserId",
                table: "WorkOrders",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
