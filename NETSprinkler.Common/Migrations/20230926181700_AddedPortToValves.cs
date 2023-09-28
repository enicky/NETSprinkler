using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NETSprinkler.Common.Migrations
{
    /// <inheritdoc />
    public partial class AddedPortToValves : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Port",
                table: "SprinklerValves",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Port",
                table: "SprinklerValves");
        }
    }
}
