using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class Adddatefieldingradestable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamMonth",
                table: "Grades");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Grades",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Grades");

            migrationBuilder.AddColumn<int>(
                name: "ExamMonth",
                table: "Grades",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
