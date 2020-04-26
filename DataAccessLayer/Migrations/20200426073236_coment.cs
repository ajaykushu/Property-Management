using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class coment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Replies_Comments_CommentsId",
                table: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_Replies_CommentsId",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "CommentsId",
                table: "Replies");

            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "Comments",
                newName: "CommentString");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "8b4dead4-4927-4f28-a55b-f4f4addd72f7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "4abc0b6b-669d-4095-852a-2bbaea3a4ea1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ConcurrencyStamp",
                value: "2fdcfbed-3db8-40ba-9fec-e5f1f76918a7");

            migrationBuilder.CreateIndex(
                name: "IX_Replies_CommentId",
                table: "Replies",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_Comments_CommentId",
                table: "Replies",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Replies_Comments_CommentId",
                table: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_Replies_CommentId",
                table: "Replies");

            migrationBuilder.RenameColumn(
                name: "CommentString",
                table: "Comments",
                newName: "Comment");

            migrationBuilder.AddColumn<long>(
                name: "CommentsId",
                table: "Replies",
                type: "bigint",
                nullable: true);

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
                name: "IX_Replies_CommentsId",
                table: "Replies",
                column: "CommentsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_Comments_CommentsId",
                table: "Replies",
                column: "CommentsId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
