using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inspections",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: false),
                    CreatedByUserName = table.Column<string>(type: "varchar(50)", nullable: true),
                    UpdatedByUserName = table.Column<string>(type: "varchar(50)", nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    DepartmentID = table.Column<int>(nullable: false),
                    CronExpression = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    PropertyId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inspections_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inspections_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CheckLists",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: false),
                    CreatedByUserName = table.Column<string>(type: "varchar(50)", nullable: true),
                    UpdatedByUserName = table.Column<string>(type: "varchar(50)", nullable: true),
                    InspectionId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    SubLocationId = table.Column<int>(nullable: true),
                    LocationId = table.Column<int>(nullable: false),
                    LocationOrder = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckLists_Inspections_InspectionId",
                        column: x => x.InspectionId,
                        principalTable: "Inspections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CheckLists_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckLists_Areas_SubLocationId",
                        column: x => x.SubLocationId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_CheckLists_InspectionId",
                table: "CheckLists",
                column: "InspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckLists_LocationId",
                table: "CheckLists",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckLists_SubLocationId",
                table: "CheckLists",
                column: "SubLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_DepartmentID",
                table: "Inspections",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_PropertyId",
                table: "Inspections",
                column: "PropertyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckLists");

            migrationBuilder.DropTable(
                name: "Inspections");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "9556f085-927b-42b8-a51c-0bcf1ca53e70");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "624c4cfc-fa42-4478-b127-5980f3ff7413");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "916fc466-13a8-4c90-b8a2-91b6605c3fd8");
        }
    }
}
