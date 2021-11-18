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
                defaultValue: new DateTime(2021, 11, 18, 19, 33, 55, 619, DateTimeKind.Local).AddTicks(6281),
                oldClrType: typeof(DateTime),
                oldType: "DATE",
                oldDefaultValue: new DateTime(2021, 11, 18, 19, 32, 17, 712, DateTimeKind.Local).AddTicks(1829));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Birthday",
                table: "Students",
                type: "DATE",
                nullable: false,
                defaultValue: new DateTime(2021, 11, 18, 19, 33, 55, 623, DateTimeKind.Local).AddTicks(3652),
                oldClrType: typeof(DateTime),
                oldType: "DATE",
                oldDefaultValue: new DateTime(2021, 11, 18, 19, 32, 17, 715, DateTimeKind.Local).AddTicks(3651));
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
                oldDefaultValue: new DateTime(2021, 11, 18, 19, 33, 55, 619, DateTimeKind.Local).AddTicks(6281));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Birthday",
                table: "Students",
                type: "DATE",
                nullable: false,
                defaultValue: new DateTime(2021, 11, 18, 19, 32, 17, 715, DateTimeKind.Local).AddTicks(3651),
                oldClrType: typeof(DateTime),
                oldType: "DATE",
                oldDefaultValue: new DateTime(2021, 11, 18, 19, 33, 55, 623, DateTimeKind.Local).AddTicks(3652));

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
    }
}
