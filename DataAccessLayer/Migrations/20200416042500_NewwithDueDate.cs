using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class NewwithDueDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "WorkOrders",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "879cacc6-55f3-4196-b73d-e89ba60a4de2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "ConcurrencyStamp", "DepartmentId", "Name", "NormalizedName" },
                values: new object[] { "34602715-88c6-489b-b44b-4d326b562b89", 2, "User", "USER" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "ConcurrencyStamp", "DepartmentId", "Name", "NormalizedName" },
                values: new object[] { "5dd499fe-320a-4948-b144-b1e74da17918", 3, "Plumber", "PLUMBER" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c0d290e0-61d3-4b9b-b083-c5e0470bfad8", "Electrician", "ELECTRICIAN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "WorkOrders");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "7f0b1b5e-7d13-45ae-a1ee-eabe24071450");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "ConcurrencyStamp", "DepartmentId", "Name", "NormalizedName" },
                values: new object[] { "3107d7e9-b655-41f4-ad13-00c026a1d3d9", 3, "Electrician", "ELECTRICIAN" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "ConcurrencyStamp", "DepartmentId", "Name", "NormalizedName" },
                values: new object[] { "0d66e4a3-9605-4b91-82a4-f5ccb802352f", 2, "Property Manager", "PROPERTY MANAGER" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "06d4e872-809f-4054-ae8d-8b53aaa2b9c3", "Plumber", "PLUMBER" });
        }
    }
}
