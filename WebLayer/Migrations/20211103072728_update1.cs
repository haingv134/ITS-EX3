using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebLayer.Migrations
{
    public partial class update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Birthday",
                table: "Students",
                type: "DATE",
                nullable: false,
                defaultValue: new DateTime(2021, 11, 3, 14, 27, 28, 182, DateTimeKind.Local).AddTicks(1943),
                oldClrType: typeof(DateTime),
                oldType: "DATE",
                oldDefaultValue: new DateTime(2021, 10, 29, 17, 7, 26, 193, DateTimeKind.Local).AddTicks(9373));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Birthday",
                table: "Students",
                type: "DATE",
                nullable: false,
                defaultValue: new DateTime(2021, 10, 29, 17, 7, 26, 193, DateTimeKind.Local).AddTicks(9373),
                oldClrType: typeof(DateTime),
                oldType: "DATE",
                oldDefaultValue: new DateTime(2021, 11, 3, 14, 27, 28, 182, DateTimeKind.Local).AddTicks(1943));
        }
    }
}
