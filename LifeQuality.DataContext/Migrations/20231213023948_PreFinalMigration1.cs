using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LifeQuality.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class PreFinalMigration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sensors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    ReadingType = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    ProfileImageUrl = table.Column<string>(type: "text", nullable: true),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    UserType = table.Column<int>(type: "integer", nullable: false),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    Doctor_Speciality = table.Column<string>(type: "text", nullable: true),
                    AdditioanlInfo = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    DoctorId = table.Column<int>(type: "integer", nullable: true),
                    PatronId = table.Column<int>(type: "integer", nullable: true),
                    PatientStatus = table.Column<int>(type: "integer", nullable: true),
                    Height = table.Column<double>(type: "double precision", nullable: true),
                    Weight = table.Column<double>(type: "double precision", nullable: true),
                    BloodType = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Speciality = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Users_PatronId",
                        column: x => x.PatronId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BloodAnalysisData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnalysisDate = table.Column<string>(type: "text", nullable: false),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    Source = table.Column<string>(type: "text", nullable: false),
                    Data = table.Column<string>(type: "text", nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SensorId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    CholesterolLevel = table.Column<double>(type: "double precision", nullable: true),
                    Triglyceride = table.Column<double>(type: "double precision", nullable: true),
                    NormalLevelRequirement = table.Column<double>(type: "double precision", nullable: true),
                    WBC = table.Column<string>(type: "text", nullable: true),
                    HGB = table.Column<string>(type: "text", nullable: true),
                    RBC = table.Column<string>(type: "text", nullable: true),
                    HCT = table.Column<string>(type: "text", nullable: true),
                    MCV = table.Column<string>(type: "text", nullable: true),
                    BloodSugarLevel = table.Column<double>(type: "double precision", nullable: true),
                    BloodGlucose = table.Column<double>(type: "double precision", nullable: true),
                    HbA1c = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodAnalysisData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BloodAnalysisData_Sensors_SensorId",
                        column: x => x.SensorId,
                        principalTable: "Sensors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BloodAnalysisData_Users_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recomendations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RecieverId = table.Column<int>(type: "integer", nullable: false),
                    AnalysisId = table.Column<int>(type: "integer", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    PatientId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recomendations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recomendations_BloodAnalysisData_AnalysisId",
                        column: x => x.AnalysisId,
                        principalTable: "BloodAnalysisData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recomendations_Users_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Recomendations_Users_RecieverId",
                        column: x => x.RecieverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BloodAnalysisData_PatientId",
                table: "BloodAnalysisData",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_BloodAnalysisData_SensorId",
                table: "BloodAnalysisData",
                column: "SensorId");

            migrationBuilder.CreateIndex(
                name: "IX_Recomendations_AnalysisId",
                table: "Recomendations",
                column: "AnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_Recomendations_PatientId",
                table: "Recomendations",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Recomendations_RecieverId",
                table: "Recomendations",
                column: "RecieverId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DoctorId",
                table: "Users",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PatronId",
                table: "Users",
                column: "PatronId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recomendations");

            migrationBuilder.DropTable(
                name: "BloodAnalysisData");

            migrationBuilder.DropTable(
                name: "Sensors");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
