using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NETSprinkler.Common.Migrations
{
    /// <inheritdoc />
    public partial class AddSprinklerToSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ValveStatus_SprinklerValve_SprinklerValveId",
                table: "ValveStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SprinklerValve",
                table: "SprinklerValve");

            migrationBuilder.RenameTable(
                name: "SprinklerValve",
                newName: "SprinklerValves");

            migrationBuilder.AddColumn<int>(
                name: "SprinklerValveId",
                table: "Schedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SprinklerValves",
                table: "SprinklerValves",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_SprinklerValveId",
                table: "Schedules",
                column: "SprinklerValveId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_SprinklerValves_SprinklerValveId",
                table: "Schedules",
                column: "SprinklerValveId",
                principalTable: "SprinklerValves",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ValveStatus_SprinklerValves_SprinklerValveId",
                table: "ValveStatus",
                column: "SprinklerValveId",
                principalTable: "SprinklerValves",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_SprinklerValves_SprinklerValveId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_ValveStatus_SprinklerValves_SprinklerValveId",
                table: "ValveStatus");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_SprinklerValveId",
                table: "Schedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SprinklerValves",
                table: "SprinklerValves");

            migrationBuilder.DropColumn(
                name: "SprinklerValveId",
                table: "Schedules");

            migrationBuilder.RenameTable(
                name: "SprinklerValves",
                newName: "SprinklerValve");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SprinklerValve",
                table: "SprinklerValve",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ValveStatus_SprinklerValve_SprinklerValveId",
                table: "ValveStatus",
                column: "SprinklerValveId",
                principalTable: "SprinklerValve",
                principalColumn: "Id");
        }
    }
}
