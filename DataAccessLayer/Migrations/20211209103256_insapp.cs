using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class insapp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Inspections");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Inspections",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "InspectionQueues",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    InspectionId = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionQueues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InspectionQueues_Inspections_InspectionId",
                        column: x => x.InspectionId,
                        principalTable: "Inspections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CheckListQueues",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CheckListId = table.Column<string>(nullable: true),
                    InspectionQueueId = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckListQueues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckListQueues_CheckLists_CheckListId",
                        column: x => x.CheckListId,
                        principalTable: "CheckLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CheckListQueues_InspectionQueues_InspectionQueueId",
                        column: x => x.InspectionQueueId,
                        principalTable: "InspectionQueues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "8cf8a0f8-c07c-4d62-8b5d-a26d4a287241");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "19bd5de9-b62f-49db-849a-1a3ef06b9cc2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "fbdad8b0-649f-4267-ba07-e786a1493c68");

            migrationBuilder.InsertData(
                table: "Menu",
                columns: new[] { "Id", "CreatedByUserName", "CreatedTime", "MenuName", "UpdatedByUserName", "UpdatedTime" },
                values: new object[] { 24L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Inspections", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "IX_CheckListQueues_CheckListId",
                table: "CheckListQueues",
                column: "CheckListId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckListQueues_InspectionQueueId",
                table: "CheckListQueues",
                column: "InspectionQueueId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionQueues_InspectionId",
                table: "InspectionQueues",
                column: "InspectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckListQueues");

            migrationBuilder.DropTable(
                name: "InspectionQueues");

            migrationBuilder.DeleteData(
                table: "Menu",
                keyColumn: "Id",
                keyValue: 24L);

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Inspections");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Inspections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "8737ef0e-760b-41fe-89c8-bee0299a7f66");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "152a1157-2f5e-4d7c-bfc8-41888b21d23f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "7a73af38-5e68-4848-9c17-5a1f00d03360");
        }
    }
}
