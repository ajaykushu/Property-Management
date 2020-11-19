using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class New_Mig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Effort_WorkOrders_WOId",
                table: "Effort");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Effort",
                table: "Effort");

            migrationBuilder.RenameTable(
                name: "Effort",
                newName: "Efforts");

            migrationBuilder.RenameIndex(
                name: "IX_Effort_WOId",
                table: "Efforts",
                newName: "IX_Efforts_WOId");

            migrationBuilder.AlterColumn<bool>(
                name: "IsEffortVisible",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Efforts",
                table: "Efforts",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "b0d6dfc0-e94e-4ea5-b3c6-1815750a252b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "610b0b7c-0b8e-49f7-8ee1-44bd79bbccb5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "1221f920-c64b-42d8-880b-6932d0f662a3");

            migrationBuilder.AddForeignKey(
                name: "FK_Efforts_WorkOrders_WOId",
                table: "Efforts",
                column: "WOId",
                principalTable: "WorkOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Efforts_WorkOrders_WOId",
                table: "Efforts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Efforts",
                table: "Efforts");

            migrationBuilder.RenameTable(
                name: "Efforts",
                newName: "Effort");

            migrationBuilder.RenameIndex(
                name: "IX_Efforts_WOId",
                table: "Effort",
                newName: "IX_Effort_WOId");

            migrationBuilder.AlterColumn<bool>(
                name: "IsEffortVisible",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Effort",
                table: "Effort",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "2a94cc6d-5a4b-4cef-9925-e1137a74b2b4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "c3fa0b18-d478-40a5-a802-bf2ae331ecb0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "dc98c651-ec40-4502-b21d-98aebd823f63");

            migrationBuilder.AddForeignKey(
                name: "FK_Effort_WorkOrders_WOId",
                table: "Effort",
                column: "WOId",
                principalTable: "WorkOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
