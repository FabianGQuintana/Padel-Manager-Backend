using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PadelManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewBusinessRulesDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Tournaments_TournamentId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Stages_StageId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Stages_Categories_CategoryId",
                table: "Stages");

            migrationBuilder.DropForeignKey(
                name: "FK_Statistics_Zones_ZoneId",
                table: "Statistics");

            migrationBuilder.DropIndex(
                name: "IX_Couples_Player1Id_Player2Id",
                table: "Couples");

            migrationBuilder.CreateIndex(
                name: "IX_Couples_Player1Id_Player2Id",
                table: "Couples",
                columns: new[] { "Player1Id", "Player2Id" },
                unique: true,
                filter: "\"DeletedAt\" IS NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Tournaments_TournamentId",
                table: "Categories",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Stages_StageId",
                table: "Matches",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stages_Categories_CategoryId",
                table: "Stages",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Statistics_Zones_ZoneId",
                table: "Statistics",
                column: "ZoneId",
                principalTable: "Zones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Tournaments_TournamentId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Stages_StageId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Stages_Categories_CategoryId",
                table: "Stages");

            migrationBuilder.DropForeignKey(
                name: "FK_Statistics_Zones_ZoneId",
                table: "Statistics");

            migrationBuilder.DropIndex(
                name: "IX_Couples_Player1Id_Player2Id",
                table: "Couples");

            migrationBuilder.CreateIndex(
                name: "IX_Couples_Player1Id_Player2Id",
                table: "Couples",
                columns: new[] { "Player1Id", "Player2Id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Tournaments_TournamentId",
                table: "Categories",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Stages_StageId",
                table: "Matches",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stages_Categories_CategoryId",
                table: "Stages",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Statistics_Zones_ZoneId",
                table: "Statistics",
                column: "ZoneId",
                principalTable: "Zones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
