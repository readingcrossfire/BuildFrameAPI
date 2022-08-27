using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ENTITYFRAMEWORK.Migrations
{
    public partial class Init_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "MENUTYPES",
                type: "NVARCHAR(200)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "LOGS",
                type: "DATETIME2",
                nullable: false,
                defaultValue: new DateTime(2022, 8, 23, 12, 11, 48, 267, DateTimeKind.Local).AddTicks(1537),
                oldClrType: typeof(DateTime),
                oldType: "DATETIME2",
                oldDefaultValue: new DateTime(2022, 8, 22, 1, 41, 28, 164, DateTimeKind.Local).AddTicks(4558));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key",
                table: "MENUTYPES");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "LOGS",
                type: "DATETIME2",
                nullable: false,
                defaultValue: new DateTime(2022, 8, 22, 1, 41, 28, 164, DateTimeKind.Local).AddTicks(4558),
                oldClrType: typeof(DateTime),
                oldType: "DATETIME2",
                oldDefaultValue: new DateTime(2022, 8, 23, 12, 11, 48, 267, DateTimeKind.Local).AddTicks(1537));
        }
    }
}
