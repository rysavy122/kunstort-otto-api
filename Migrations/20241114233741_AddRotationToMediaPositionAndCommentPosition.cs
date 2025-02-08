using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Migrations
{
    public partial class AddRotationToMediaPositionAndCommentPosition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Height",
                table: "MediaPositions",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Rotation",
                table: "MediaPositions",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Width",
                table: "MediaPositions",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Rotation",
                table: "CommentPositions",
                type: "float",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "MediaPositions");

            migrationBuilder.DropColumn(
                name: "Rotation",
                table: "MediaPositions");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "MediaPositions");

            migrationBuilder.DropColumn(
                name: "Rotation",
                table: "CommentPositions");
        }
    }
}
