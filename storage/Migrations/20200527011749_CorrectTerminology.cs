using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GameTrove.Storage.Migrations
{
    public partial class CorrectTerminology : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlatformGames");

            migrationBuilder.DropColumn(
                name: "PlatformGameId",
                table: "PlatformGameImages");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Games");

            migrationBuilder.AddColumn<Guid>(
                name: "GameId",
                table: "PlatformGameImages",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Games",
                nullable: false,
                defaultValueSql: "newid()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Games",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PlatformId",
                table: "Games",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "Registered",
                table: "Games",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "TitleId",
                table: "Games",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Titles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Subtitle = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Titles", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Titles");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "PlatformGameImages");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "PlatformId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Registered",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "TitleId",
                table: "Games");

            migrationBuilder.AddColumn<Guid>(
                name: "PlatformGameId",
                table: "PlatformGameImages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Games",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldDefaultValueSql: "newid()");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Games",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Games",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PlatformGames",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlatformId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Registered = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformGames", x => x.Id);
                });
        }
    }
}
