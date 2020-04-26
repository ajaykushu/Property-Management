using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class forcomet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentPath",
                table: "Comments");

            migrationBuilder.AddColumn<long>(
                name: "ReplyById",
                table: "Replies",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CommentById",
                table: "Comments",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "3e95ee59-eae3-43d9-b3a4-a0fd29bdae4e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "6e983dc5-0de6-42cc-ae31-4624e4266507");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "f8b4d303-ff63-41ae-bbc0-4fd9237ee389");

            migrationBuilder.CreateIndex(
                name: "IX_Replies_ReplyById",
                table: "Replies",
                column: "ReplyById");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CommentById",
                table: "Comments",
                column: "CommentById");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_CommentById",
                table: "Comments",
                column: "CommentById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_AspNetUsers_ReplyById",
                table: "Replies",
                column: "ReplyById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_CommentById",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Replies_AspNetUsers_ReplyById",
                table: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_Replies_ReplyById",
                table: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_Comments_CommentById",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ReplyById",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "CommentById",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentPath",
                table: "Comments",
                type: "varchar(300)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "26cb3ee8-dd96-441a-8028-6244c8a11d34");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "868ad915-3db8-4035-8a22-9c1869eedc8b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "2ff788cb-1577-476a-931d-39f3b086609e");
        }
    }
}
