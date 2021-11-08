using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebLayer.Migrations
{
    public partial class midifiedEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubjectCode",
                table: "Subjects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Birthday",
                table: "Students",
                type: "DATE",
                nullable: false,
                defaultValue: new DateTime(2021, 11, 5, 11, 43, 29, 974, DateTimeKind.Local).AddTicks(1536),
                oldClrType: typeof(DateTime),
                oldType: "DATE",
                oldDefaultValue: new DateTime(2021, 11, 3, 14, 29, 20, 394, DateTimeKind.Local).AddTicks(7036));

            migrationBuilder.AddColumn<string>(
                name: "StudentCode",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubjectCode",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "StudentCode",
                table: "Students");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Birthday",
                table: "Students",
                type: "DATE",
                nullable: false,
                defaultValue: new DateTime(2021, 11, 3, 14, 29, 20, 394, DateTimeKind.Local).AddTicks(7036),
                oldClrType: typeof(DateTime),
                oldType: "DATE",
                oldDefaultValue: new DateTime(2021, 11, 5, 11, 43, 29, 974, DateTimeKind.Local).AddTicks(1536));
        }
    }
}
