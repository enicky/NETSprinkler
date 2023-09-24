using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NETSprinkler.Common.Migrations
{
    /// <inheritdoc />
    public partial class AddValve : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SprinklerId",
                table: "ValveStatus",
                newName: "SprinklerValveId");

            migrationBuilder.CreateTable(
                name: "SprinklerValve",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SprinklerValve", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ValveStatus_SprinklerValveId",
                table: "ValveStatus",
                column: "SprinklerValveId",
                unique: true,
                filter: "[SprinklerValveId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ValveStatus_SprinklerValve_SprinklerValveId",
                table: "ValveStatus",
                column: "SprinklerValveId",
                principalTable: "SprinklerValve",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ValveStatus_SprinklerValve_SprinklerValveId",
                table: "ValveStatus");

            migrationBuilder.DropTable(
                name: "SprinklerValve");

            migrationBuilder.DropIndex(
                name: "IX_ValveStatus_SprinklerValveId",
                table: "ValveStatus");

            migrationBuilder.RenameColumn(
                name: "SprinklerValveId",
                table: "ValveStatus",
                newName: "SprinklerId");
        }
    }
}
