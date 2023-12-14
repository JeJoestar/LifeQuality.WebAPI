using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeQuality.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class FixRecommendation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recomendations_Users_PatientId",
                table: "Recomendations");

            migrationBuilder.DropIndex(
                name: "IX_Recomendations_PatientId",
                table: "Recomendations");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Recomendations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "Recomendations",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recomendations_PatientId",
                table: "Recomendations",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recomendations_Users_PatientId",
                table: "Recomendations",
                column: "PatientId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
