using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NETSprinkler.Common.Migrations
{
    /// <inheritdoc />
    public partial class AddedEnableFlagOnValve : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "SprinklerValves",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "SprinklerValves");
        }
    }
}
