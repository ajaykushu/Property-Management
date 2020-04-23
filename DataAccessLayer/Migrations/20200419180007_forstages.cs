using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DataAccessLayer.Migrations
{
    public partial class forstages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "12c8c0d6-d626-41a1-918e-c499750f4b5e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "e46b0f1b-a3ec-458f-8726-2289f1d231ba");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "7ad5db90-2a38-443c-9e76-5145ca674ae5");

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "StageCode", "StageDescription" },
                values: new object[] { "OPEN", "Work Order Open State" });

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "StageCode", "StageDescription" },
                values: new object[] { "BIDACCEPTED", "Bid Sucessfull" });

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "StageCode", "StageDescription" },
                values: new object[] { "INPROGRESS", "Work Order in Progress" });

            migrationBuilder.InsertData(
                table: "Stages",
                columns: new[] { "Id", "CreatedByUserName", "CreatedTime", "StageCode", "StageDescription", "UpdatedByUserName", "UpdatedTime" },
                values: new object[] { 4, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "COMPLETED", "Work Order Completed", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "046980e9-30c9-40ca-b8d4-e01479e202d5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "8f346fb3-74ef-4006-931f-bfcc067631d2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "71bf7d0e-6769-44ae-ac37-dfcdd355a492");

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "StageCode", "StageDescription" },
                values: new object[] { "INITWO", "Work Order Created" });

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "StageCode", "StageDescription" },
                values: new object[] { "WOPROG", "Work Order in Progress" });

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "StageCode", "StageDescription" },
                values: new object[] { "WOCOMP", "Work Order Completed" });
        }
    }
}