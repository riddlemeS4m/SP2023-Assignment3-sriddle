using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SP2023_Assignment3_sriddle.Data.Migrations
{
    public partial class updatedCast : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CharacterName",
                table: "Cast",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CharacterName",
                table: "Cast");
        }
    }
}
