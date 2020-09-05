using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class newTableForRecurringWo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_WorkOrders_ParentWorkOrderId",
                table: "WorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_ParentWorkOrderId",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "CronExpression",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "EndAfterCount",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "ParentWorkOrderId",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "Recurring",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "RecurringEndDate",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "RecurringStartDate",
                table: "WorkOrders");

            migrationBuilder.AddColumn<string>(
                name: "ParentWoId",
                table: "WorkOrders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecurringWOId",
                table: "WOAttachments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecurringWOId",
                table: "Comments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RecurringWOs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: false),
                    CreatedByUserName = table.Column<string>(type: "varchar(50)", nullable: true),
                    UpdatedByUserName = table.Column<string>(type: "varchar(50)", nullable: true),
                    PropertyId = table.Column<long>(nullable: false),
                    ItemId = table.Column<int>(nullable: false),
                    IssueId = table.Column<int>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(type: "varchar(200)", nullable: true),
                    RequestedBy = table.Column<string>(type: "varchar(50)", nullable: true),
                    AssignedToId = table.Column<long>(nullable: true),
                    AssignedToDeptId = table.Column<int>(nullable: true),
                    VendorId = table.Column<int>(nullable: true),
                    DueDate = table.Column<DateTime>(nullable: false),
                    LocationId = table.Column<int>(nullable: true),
                    SubLocationId = table.Column<int>(nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    CronExpression = table.Column<string>(nullable: true),
                    RecurringEndDate = table.Column<DateTime>(nullable: true),
                    RecurringStartDate = table.Column<DateTime>(nullable: true),
                    ParentWorkOrderId = table.Column<string>(nullable: true),
                    EndAfterCount = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurringWOs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecurringWOs_Departments_AssignedToDeptId",
                        column: x => x.AssignedToDeptId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecurringWOs_AspNetUsers_AssignedToId",
                        column: x => x.AssignedToId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecurringWOs_Issues_IssueId",
                        column: x => x.IssueId,
                        principalTable: "Issues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecurringWOs_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecurringWOs_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecurringWOs_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecurringWOs_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecurringWOs_Areas_SubLocationId",
                        column: x => x.SubLocationId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecurringWOs_Vendor_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "76fe1008-2b3d-4dc0-8f06-cb5b87fd4b82");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "ee5ab939-3d26-4dc7-a643-7a35bdde27f4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "7d8f66f6-c391-4983-a387-85c9383b40ca");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_ParentWoId",
                table: "WorkOrders",
                column: "ParentWoId");

            migrationBuilder.CreateIndex(
                name: "IX_WOAttachments_RecurringWOId",
                table: "WOAttachments",
                column: "RecurringWOId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_RecurringWOId",
                table: "Comments",
                column: "RecurringWOId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringWOs_AssignedToDeptId",
                table: "RecurringWOs",
                column: "AssignedToDeptId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringWOs_AssignedToId",
                table: "RecurringWOs",
                column: "AssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringWOs_IssueId",
                table: "RecurringWOs",
                column: "IssueId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringWOs_ItemId",
                table: "RecurringWOs",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringWOs_LocationId",
                table: "RecurringWOs",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringWOs_PropertyId",
                table: "RecurringWOs",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringWOs_StatusId",
                table: "RecurringWOs",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringWOs_SubLocationId",
                table: "RecurringWOs",
                column: "SubLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringWOs_VendorId",
                table: "RecurringWOs",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_RecurringWOs_RecurringWOId",
                table: "Comments",
                column: "RecurringWOId",
                principalTable: "RecurringWOs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WOAttachments_RecurringWOs_RecurringWOId",
                table: "WOAttachments",
                column: "RecurringWOId",
                principalTable: "RecurringWOs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrders_RecurringWOs_ParentWoId",
                table: "WorkOrders",
                column: "ParentWoId",
                principalTable: "RecurringWOs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_RecurringWOs_RecurringWOId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_WOAttachments_RecurringWOs_RecurringWOId",
                table: "WOAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_RecurringWOs_ParentWoId",
                table: "WorkOrders");

            migrationBuilder.DropTable(
                name: "RecurringWOs");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_ParentWoId",
                table: "WorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_WOAttachments_RecurringWOId",
                table: "WOAttachments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_RecurringWOId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ParentWoId",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "RecurringWOId",
                table: "WOAttachments");

            migrationBuilder.DropColumn(
                name: "RecurringWOId",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "CronExpression",
                table: "WorkOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EndAfterCount",
                table: "WorkOrders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentWorkOrderId",
                table: "WorkOrders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Recurring",
                table: "WorkOrders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "RecurringEndDate",
                table: "WorkOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RecurringStartDate",
                table: "WorkOrders",
                type: "datetime2",
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
    }
}
