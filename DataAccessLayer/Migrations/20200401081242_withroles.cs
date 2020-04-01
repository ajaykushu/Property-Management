using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class withroles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkerTyperUserMap");

            migrationBuilder.DropTable(
                name: "WorkerType");

            migrationBuilder.DropTable(
                name: "PayScale");

            migrationBuilder.DropColumn(
                name: "StageShortHand",
                table: "Stages");

            migrationBuilder.RenameColumn(
                name: "isPrimary",
                table: "UserProperties",
                newName: "IsPrimary");

            migrationBuilder.AlterColumn<string>(
                name: "StageCode",
                table: "Stages",
                type: "varchar(7)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(5)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ParentId",
                table: "Comments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "AspNetRoles",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "2e2b589a-8cce-4cfc-ae21-06c76c5a8385");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "605fa7c4-8d21-4d85-8548-a432e7ccade7");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentId",
                table: "Comments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_DepartmentId",
                table: "AspNetRoles",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_Department_DepartmentId",
                table: "AspNetRoles",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_ParentId",
                table: "Comments",
                column: "ParentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_Department_DepartmentId",
                table: "AspNetRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_ParentId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ParentId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoles_DepartmentId",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "AspNetRoles");

            migrationBuilder.RenameColumn(
                name: "IsPrimary",
                table: "UserProperties",
                newName: "isPrimary");

            migrationBuilder.AlterColumn<string>(
                name: "StageCode",
                table: "Stages",
                type: "varchar(5)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(7)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StageShortHand",
                table: "Stages",
                type: "varchar(7)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PayScale",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Billingtype = table.Column<string>(type: "varchar(10)", nullable: true),
                    Code = table.Column<string>(type: "varchar(5)", nullable: true),
                    HourlyCost = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayScale", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkerType",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    PayScaleId = table.Column<int>(type: "int", nullable: false),
                    TypeName = table.Column<string>(type: "varchar(30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkerType_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkerType_PayScale_PayScaleId",
                        column: x => x.PayScaleId,
                        principalTable: "PayScale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkerTyperUserMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<long>(type: "bigint", nullable: false),
                    WorkerTypeId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerTyperUserMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkerTyperUserMap_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkerTyperUserMap_WorkerType_WorkerTypeId",
                        column: x => x.WorkerTypeId,
                        principalTable: "WorkerType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "0c23d9fe-34d2-440e-a1cc-ea4d0e98a7c1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "5463ca92-4a29-4ed6-a473-e19780dc80b4");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerType_DepartmentId",
                table: "WorkerType",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerType_PayScaleId",
                table: "WorkerType",
                column: "PayScaleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerTyperUserMap_ApplicationUserId",
                table: "WorkerTyperUserMap",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerTyperUserMap_WorkerTypeId",
                table: "WorkerTyperUserMap",
                column: "WorkerTypeId");
        }
    }
}
