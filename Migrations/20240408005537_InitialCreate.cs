using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastEvaluator.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnemometerReadings",
                columns: table => new
                {
                    ReadingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReadingDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Hour = table.Column<TimeOnly>(type: "time", nullable: false),
                    WindSpeed = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WindGusts = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WindDirection = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnemometerReadings", x => x.ReadingID);
                });

            migrationBuilder.CreateTable(
                name: "WeatherForecasts",
                columns: table => new
                {
                    ForecastID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RetrievalDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ForecastForDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ModelType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherForecasts", x => x.ForecastID);
                });

            migrationBuilder.CreateTable(
                name: "ForecastsDetails",
                columns: table => new
                {
                    HourID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ForecastID = table.Column<int>(type: "int", nullable: false),
                    Hour = table.Column<TimeOnly>(type: "time", nullable: false),
                    WindSpeed = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WindGusts = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WindDirection = table.Column<int>(type: "int", nullable: false),
                    WeatherForecastForecastID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForecastsDetails", x => x.HourID);
                    table.ForeignKey(
                        name: "FK_ForecastsDetails_WeatherForecasts_WeatherForecastForecastID",
                        column: x => x.WeatherForecastForecastID,
                        principalTable: "WeatherForecasts",
                        principalColumn: "ForecastID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ForecastsDetails_WeatherForecastForecastID",
                table: "ForecastsDetails",
                column: "WeatherForecastForecastID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnemometerReadings");

            migrationBuilder.DropTable(
                name: "ForecastsDetails");

            migrationBuilder.DropTable(
                name: "WeatherForecasts");
        }
    }
}
