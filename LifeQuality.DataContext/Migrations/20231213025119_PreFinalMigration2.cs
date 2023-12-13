using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeQuality.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class PreFinalMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_PatronId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_PatronId",
                table: "Users",
                column: "PatronId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_PatronId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_PatronId",
                table: "Users",
                column: "PatronId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
