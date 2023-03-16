#region using
using Microsoft.EntityFrameworkCore.Migrations;
#endregion

#nullable disable

namespace DataBase.Migrations
{
    public partial class DbMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region Tables
            #region Leg
            migrationBuilder.CreateTable(
                name: "Legs",
                columns: table => new
                {
                    LegId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<double>(type: "float", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsEmpty = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Legs", x => x.LegId);
                });
            #endregion

            #region Airplane
            migrationBuilder.CreateTable(
                name: "Airplanes",
                columns: table => new
                {
                    AirplaneId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    SerialNumber = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyName = table.Column<int>(type: "int", nullable: false),
                    Destenation = table.Column<int>(type: "int", nullable: false),
                    Origin = table.Column<int>(type: "int", nullable: false),
                    IsLanding = table.Column<bool>(type: "bit", nullable: true),
                    MaxPassengers = table.Column<int>(type: "int", nullable: false),
                    CrewMembers = table.Column<int>(type: "int", nullable: false),
                    Width = table.Column<double>(type: "float", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    CurrentWeight = table.Column<int>(type: "int", nullable: false),
                    MaxWeight = table.Column<int>(type: "int", nullable: false),
                    MaxSpeed = table.Column<int>(type: "int", nullable: false),
                    CurrentFuelState = table.Column<int>(type: "int", nullable: false),
                    LegId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airplanes", x => x.AirplaneId);
                    table.ForeignKey(
                        name: "FK_Airplanes_Legs_LegId",
                        column: x => x.LegId,
                        principalTable: "Legs",
                        principalColumn: "LegId",
                        onDelete: ReferentialAction.Cascade);
                });
            #endregion

            #region Logger
            migrationBuilder.CreateTable(
                name: "Loggers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AirplaneId = table.Column<int>(type: "int", nullable: false),
                    LegId = table.Column<int>(type: "int", nullable: true),
                    In = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Out = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loggers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loggers_Airplanes_AirplaneId",
                        column: x => x.AirplaneId,
                        principalTable: "Airplanes",
                        principalColumn: "AirplaneId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Loggers_Legs_LegId",
                        column: x => x.LegId,
                        principalTable: "Legs",
                        principalColumn: "LegId");
                });
            #endregion

            #endregion

            #region Tables Connections

            #region Airplane
            migrationBuilder.CreateIndex(
                name: "IX_Airplanes_LegId",
                table: "Airplanes",
                column: "LegId");
            #endregion

            #region Logger
            migrationBuilder.CreateIndex(
                name: "IX_Loggers_AirplaneId",
                table: "Loggers",
                column: "AirplaneId");

            migrationBuilder.CreateIndex(
                name: "IX_Loggers_LegId",
                table: "Loggers",
                column: "LegId");
            #endregion

            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Loggers");

            migrationBuilder.DropTable(
                name: "Airplanes");

            migrationBuilder.DropTable(
                name: "Legs");
        }
    }
}


