using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class Efort : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TotalEffort",
                table: "WorkOrders",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsEffortVisible",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Effort",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    TaxBO = table.Column<int>(nullable: false),
                    Repair = table.Column<int>(nullable: false),
                    WOId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Effort", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Effort_WorkOrders_WOId",
                        column: x => x.WOId,
                        principalTable: "WorkOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "999a8642-62e7-4bef-a1f5-56405d6a6876");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "0d796dc5-69a3-4688-a6d8-e825d4b2d19d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "188e2125-891d-432d-805c-0594a467fd1b");

            migrationBuilder.CreateIndex(
                name: "IX_Effort_WOId",
                table: "Effort",
                column: "WOId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Effort");

            migrationBuilder.DropColumn(
                name: "TotalEffort",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "IsEffortVisible",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "ea8fb6ad-b36f-4410-84f6-61560c0d47a8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "03d27278-496a-4e9a-811a-962a8d58878c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "83b6c6ea-86c4-4bbb-a320-b921fc634f96");
        }
    }
}
