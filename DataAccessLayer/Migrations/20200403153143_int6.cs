using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class int6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_AspNetUsers_AssignedToId1",
                table: "WorkOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_AspNetRoles_AssignedToRoleId1",
                table: "WorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_AssignedToId1",
                table: "WorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_AssignedToRoleId1",
                table: "WorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_Properties_PropertyName",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "AssignedToId1",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "AssignedToRoleId1",
                table: "WorkOrders");

            migrationBuilder.AlterColumn<string>(
                name: "Street",
                table: "Properties",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "PropertyName",
                table: "Properties",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "HouseNumber",
                table: "Properties",
                type: "varchar(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(10)");

            migrationBuilder.AlterColumn<string>(
                name: "MenuName",
                table: "Menu",
                type: "varchar(30)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(30)");

            migrationBuilder.AlterColumn<string>(
                name: "Language",
                table: "Languages",
                type: "varchar(40)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(40)");

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentName",
                table: "Department",
                type: "varchar(30)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(30)");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "01e41dab-7d70-43f7-a051-45da6819a2ad");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "2c7d2e43-78f0-4049-8ef0-af461916128b");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_PropertyName",
                table: "Properties",
                column: "PropertyName",
                unique: true,
                filter: "[PropertyName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Properties_PropertyName",
                table: "Properties");

            migrationBuilder.AddColumn<long>(
                name: "AssignedToId1",
                table: "WorkOrders",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AssignedToRoleId1",
                table: "WorkOrders",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Street",
                table: "Properties",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PropertyName",
                table: "Properties",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HouseNumber",
                table: "Properties",
                type: "varchar(10)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MenuName",
                table: "Menu",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Language",
                table: "Languages",
                type: "varchar(40)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(40)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentName",
                table: "Department",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true);

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
                name: "IX_WorkOrders_AssignedToId1",
                table: "WorkOrders",
                column: "AssignedToId1");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_AssignedToRoleId1",
                table: "WorkOrders",
                column: "AssignedToRoleId1");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_PropertyName",
                table: "Properties",
                column: "PropertyName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrders_AspNetUsers_AssignedToId1",
                table: "WorkOrders",
                column: "AssignedToId1",
                principalTable: "AspNetUsers",
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
    }
}
