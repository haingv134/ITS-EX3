using System;
using Bogus;
using DatabaseLayer.Entity;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebLayer.Migrations
{
    public partial class seedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "Subjects",
                type: "DATE",
                nullable: false,
                defaultValue: new DateTime(2021, 11, 18, 19, 55, 54, 410, DateTimeKind.Local).AddTicks(4076),
                oldClrType: typeof(DateTime),
                oldType: "DATE",
                oldDefaultValue: new DateTime(2021, 11, 18, 19, 32, 17, 712, DateTimeKind.Local).AddTicks(1829));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Birthday",
                table: "Students",
                type: "DATE",
                nullable: false,
                defaultValue: new DateTime(2021, 11, 18, 19, 55, 54, 413, DateTimeKind.Local).AddTicks(3304),
                oldClrType: typeof(DateTime),
                oldType: "DATE",
                oldDefaultValue: new DateTime(2021, 11, 18, 19, 32, 17, 715, DateTimeKind.Local).AddTicks(3651));

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
            // this is for seed init data
            Randomizer.Seed = new Random(8675309);
            var fakerStudent = new Faker<Student>();
            fakerStudent.RuleFor(model => model.Name, f => f.Random.Word());
            fakerStudent.RuleFor(model => model.Birthday, f => f.Date.Between(new DateTime(2000, 1, 1), new DateTime(2020, 12, 30)));
            fakerStudent.RuleFor(model => model.Gender, f => f.Random.Bool());
            fakerStudent.RuleFor(model => model.ExtraInfor, f => f.Lorem.Paragraph(4));

            for (int i = 0; i < 100; i++)
            {
                migrationBuilder.InsertData(
                    table: "Classes",
                    columns: new[]{
                        "Name"
                    },
                    values: new object[]{
                        $"SE{i}{new Random().Next(1,9)}{i}{i+1}"
                    }
                );
                var student = fakerStudent.Generate();
                migrationBuilder.InsertData(
                    table: "Students",
                    columns: new[]{
                        "StudentCode", "Name", "Birthday", "Gender", "ExtraInfor"
                    },
                    values: new object[]{
                        $"HE{i*i}{new Random().Next(1,9)}{i}{i+1}",
                        student.Name, 
                        student.Birthday, 
                        student.Gender, 
                        student.ExtraInfor
                    }
                );
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "Subjects",
                type: "DATE",
                nullable: false,
                defaultValue: new DateTime(2021, 11, 18, 19, 32, 17, 712, DateTimeKind.Local).AddTicks(1829),
                oldClrType: typeof(DateTime),
                oldType: "DATE",
                oldDefaultValue: new DateTime(2021, 11, 18, 19, 55, 54, 410, DateTimeKind.Local).AddTicks(4076));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Birthday",
                table: "Students",
                type: "DATE",
                nullable: false,
                defaultValue: new DateTime(2021, 11, 18, 19, 32, 17, 715, DateTimeKind.Local).AddTicks(3651),
                oldClrType: typeof(DateTime),
                oldType: "DATE",
                oldDefaultValue: new DateTime(2021, 11, 18, 19, 55, 54, 413, DateTimeKind.Local).AddTicks(3304));
        }
    }
}
