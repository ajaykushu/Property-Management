using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DataAccessLayer.Migrations
{
    public partial class withupdation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRecurring",
                table: "WorkOrders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "VendorId",
                table: "WorkOrders",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Vendor",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: false),
                    CreatedByUserName = table.Column<string>(type: "varchar(50)", nullable: true),
                    UpdatedByUserName = table.Column<string>(type: "varchar(50)", nullable: true),
                    VendorName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendor", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "1c4bf0e0-01e5-418c-8886-e07eb77f8cd6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "cf644c7d-d97e-4e97-bdbc-a85fbec87280");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "08dc6dc4-0001-40b2-aba6-b8fff4322568");

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "StageCode", "StageDescription" },
                values: new object[] { "ADCM", "Add Comment Only" });

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "StageCode", "StageDescription" },
                values: new object[] { "BINE", "BID NEEDED" });

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "StageCode", "StageDescription" },
                values: new object[] { "BIRE", "BID Recieved" });

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "StageCode", "StageDescription" },
                values: new object[] { "BIRE", "Bid Requested" });

            migrationBuilder.InsertData(
                table: "Stages",
                columns: new[] { "Id", "CreatedByUserName", "CreatedTime", "StageCode", "StageDescription", "UpdatedByUserName", "UpdatedTime" },
                values: new object[,]
                {
                    { 18, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "WOOR", "Work Ordered", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 17, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "WOPR", "Work In Progress", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 15, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "WOAS", "Work Assigned", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "REAS", "Ready To Assign", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ORMA", "Order Materials", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "NEWO", "New Work Order", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "HOLD", "Hold", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "FISC", "Finalize Scope", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "DAIL", "Daily", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CONI", "Complete, Need Inspection", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "COBQ", "Complete but bad Quality", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "PEND", "Pending", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "COMP", "Complete", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_VendorId",
                table: "WorkOrders",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrders_Vendor_VendorId",
                table: "WorkOrders",
                column: "VendorId",
                principalTable: "Vendor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_Vendor_VendorId",
                table: "WorkOrders");

            migrationBuilder.DropTable(
                name: "Vendor");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_VendorId",
                table: "WorkOrders");

            migrationBuilder.DeleteData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DropColumn(
                name: "IsRecurring",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "WorkOrders");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "2c2e3813-6e95-4f48-b714-d02ff0b71d33");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "85b55bf4-95fc-455a-af1b-c70a04e6232d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "e4c0235d-8e5d-4898-b40d-a11a66ee8963");

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
                values: new object[] { "BID ACCEPTED", "Bid Sucessfull" });

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "StageCode", "StageDescription" },
                values: new object[] { "IN PROGRESS", "Work Order in Progress" });

            migrationBuilder.UpdateData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "StageCode", "StageDescription" },
                values: new object[] { "COMPLETED", "Work Order Completed" });
        }
    }
}