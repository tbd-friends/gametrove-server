using Microsoft.EntityFrameworkCore.Migrations;

namespace GameTrove.Storage.Migrations
{
    public partial class RemoveIdentifier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Identifier",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Identifier",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
