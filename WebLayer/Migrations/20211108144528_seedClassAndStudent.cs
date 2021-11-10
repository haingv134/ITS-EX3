using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Bogus;
using DatabaseLayer.Entity;

namespace WebLayer.Migrations
{
    public partial class seedClassAndStudent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Birthday",
                table: "Students",
                type: "DATE",
                nullable: false,
                defaultValue: new DateTime(2021, 11, 8, 21, 45, 28, 450, DateTimeKind.Local).AddTicks(6781),
                oldClrType: typeof(DateTime),
                oldType: "DATE",
                oldDefaultValue: new DateTime(2021, 11, 5, 11, 43, 29, 974, DateTimeKind.Local).AddTicks(1536));

            migrationBuilder.AddColumn<string>(
                name: "ExtraInfor",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);


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
            migrationBuilder.DropColumn(
                name: "ExtraInfor",
                table: "Students");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Birthday",
                table: "Students",
                type: "DATE",
                nullable: false,
                defaultValue: new DateTime(2021, 11, 5, 11, 43, 29, 974, DateTimeKind.Local).AddTicks(1536),
                oldClrType: typeof(DateTime),
                oldType: "DATE",
                oldDefaultValue: new DateTime(2021, 11, 8, 21, 45, 28, 450, DateTimeKind.Local).AddTicks(6781));
        }
    }
}
