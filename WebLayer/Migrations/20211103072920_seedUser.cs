using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebLayer.Migrations
{
    public partial class seedUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Birthday",
                table: "Students",
                type: "DATE",
                nullable: false,
                defaultValue: new DateTime(2021, 11, 3, 14, 29, 20, 394, DateTimeKind.Local).AddTicks(7036),
                oldClrType: typeof(DateTime),
                oldType: "DATE",
                oldDefaultValue: new DateTime(2021, 11, 3, 14, 27, 28, 182, DateTimeKind.Local).AddTicks(1943));

            for (int i = 1; i < 150; i++)
            {
                migrationBuilder.InsertData(
                    table: "Users",
                    columns: new[] {
                        "Id",
                        "UserName",
                        "Email",
                        "SecurityStamp",
                        "EmailConfirmed",
                        "PhoneNumberConfirmed",
                        "TwoFactorEnabled",
                        "AccessFailedCount",
                        "LockoutEnabled",
                        "Address"
                    },
                    values: new object[]{
                        Guid.NewGuid().ToString(),
                        "Users-"+i.ToString("D3"),
                        $"email{i.ToString("D3") }@example.com",
                        Guid.NewGuid().ToString(),
                        false,
                        false,
                        false,
                        0,
                        false,
                        "default_address"
                    }
                );
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Birthday",
                table: "Students",
                type: "DATE",
                nullable: false,
                defaultValue: new DateTime(2021, 11, 3, 14, 27, 28, 182, DateTimeKind.Local).AddTicks(1943),
                oldClrType: typeof(DateTime),
                oldType: "DATE",
                oldDefaultValue: new DateTime(2021, 11, 3, 14, 29, 20, 394, DateTimeKind.Local).AddTicks(7036));
        }
    }
}
