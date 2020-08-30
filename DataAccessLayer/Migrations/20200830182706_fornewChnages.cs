using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class fornewChnages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CronExpression",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "EndAfterCount",
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
                value: "108f8f66-ae91-46db-be33-b2d45940c0c1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "93ba2d51-7bb1-494f-82bb-75543c0191ad");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "53753b36-ca44-4d2c-853f-8328435f0cbb");

            migrationBuilder.InsertData(
                table: "Menu",
                columns: new[] { "Id", "CreatedByUserName", "CreatedTime", "MenuName", "UpdatedByUserName", "UpdatedTime" },
                values: new object[] { 19L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Recurring_WO", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

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

            migrationBuilder.DeleteData(
                table: "Menu",
                keyColumn: "Id",
                keyValue: 19L);

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
