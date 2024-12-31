using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VeriTabaniOdev.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "scoringsystem",
                columns: table => new
                {
                    sportid = table.Column<int>(type: "integer", nullable: false),
                    pointswin = table.Column<int>(type: "integer", nullable: true, defaultValue: 3),
                    pointsdraw = table.Column<int>(type: "integer", nullable: true, defaultValue: 1),
                    pointsloss = table.Column<int>(type: "integer", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("scoringsystem_pkey", x => x.sportid);
                });

            migrationBuilder.CreateTable(
                name: "sports",
                columns: table => new
                {
                    sportid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sportname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("sports_pkey", x => x.sportid);
                });

            migrationBuilder.CreateTable(
                name: "leagues",
                columns: table => new
                {
                    leagueid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    leaguename = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    sportid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("leagues_pkey", x => x.leagueid);
                    table.ForeignKey(
                        name: "leagues_sportid_fkey",
                        column: x => x.sportid,
                        principalTable: "sports",
                        principalColumn: "sportid");
                });

            migrationBuilder.CreateTable(
                name: "positions",
                columns: table => new
                {
                    positionid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    positionname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    sportid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("positions_pkey", x => x.positionid);
                    table.ForeignKey(
                        name: "positions_sportid_fkey",
                        column: x => x.sportid,
                        principalTable: "sports",
                        principalColumn: "sportid");
                });

            migrationBuilder.CreateTable(
                name: "teams",
                columns: table => new
                {
                    teamid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    teamname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    leagueid = table.Column<int>(type: "integer", nullable: true),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    points = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    goalsscored = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    goalsconceded = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    goaldifference = table.Column<int>(type: "integer", nullable: true, computedColumnSql: "(goalsscored - goalsconceded)", stored: true),
                    wins = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    draws = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    losses = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    leagueposition = table.Column<int>(type: "integer", nullable: true),
                    coachname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    clubpresident = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("teams_pkey", x => x.teamid);
                    table.ForeignKey(
                        name: "teams_leagueid_fkey",
                        column: x => x.leagueid,
                        principalTable: "leagues",
                        principalColumn: "leagueid");
                });

            migrationBuilder.CreateTable(
                name: "fixtures",
                columns: table => new
                {
                    matchid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    leagueid = table.Column<int>(type: "integer", nullable: true),
                    hometeamid = table.Column<int>(type: "integer", nullable: true),
                    awayteamid = table.Column<int>(type: "integer", nullable: true),
                    matchdate = table.Column<DateOnly>(type: "date", nullable: true),
                    homescore = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    awayscore = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    matchresult = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true, computedColumnSql: "\nCASE\n    WHEN (homescore > awayscore) THEN 'HomeWin'::text\n    WHEN (homescore < awayscore) THEN 'AwayWin'::text\n    WHEN (homescore = awayscore) THEN 'Draw'::text\n    ELSE 'Pending'::text\nEND", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("fixtures_pkey", x => x.matchid);
                    table.ForeignKey(
                        name: "fixtures_awayteamid_fkey",
                        column: x => x.awayteamid,
                        principalTable: "teams",
                        principalColumn: "teamid");
                    table.ForeignKey(
                        name: "fixtures_hometeamid_fkey",
                        column: x => x.hometeamid,
                        principalTable: "teams",
                        principalColumn: "teamid");
                    table.ForeignKey(
                        name: "fixtures_leagueid_fkey",
                        column: x => x.leagueid,
                        principalTable: "leagues",
                        principalColumn: "leagueid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "players",
                columns: table => new
                {
                    playerid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    playername = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    teamid = table.Column<int>(type: "integer", nullable: true),
                    positionid = table.Column<int>(type: "integer", nullable: true),
                    goals = table.Column<int>(type: "integer", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("players_pkey", x => x.playerid);
                    table.ForeignKey(
                        name: "players_positionid_fkey",
                        column: x => x.positionid,
                        principalTable: "positions",
                        principalColumn: "positionid");
                    table.ForeignKey(
                        name: "players_teamid_fkey",
                        column: x => x.teamid,
                        principalTable: "teams",
                        principalColumn: "teamid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_fixtures_awayteamid",
                table: "fixtures",
                column: "awayteamid");

            migrationBuilder.CreateIndex(
                name: "IX_fixtures_hometeamid",
                table: "fixtures",
                column: "hometeamid");

            migrationBuilder.CreateIndex(
                name: "IX_fixtures_leagueid",
                table: "fixtures",
                column: "leagueid");

            migrationBuilder.CreateIndex(
                name: "IX_leagues_sportid",
                table: "leagues",
                column: "sportid");

            migrationBuilder.CreateIndex(
                name: "IX_players_positionid",
                table: "players",
                column: "positionid");

            migrationBuilder.CreateIndex(
                name: "IX_players_teamid",
                table: "players",
                column: "teamid");

            migrationBuilder.CreateIndex(
                name: "IX_positions_sportid",
                table: "positions",
                column: "sportid");

            migrationBuilder.CreateIndex(
                name: "IX_teams_leagueid",
                table: "teams",
                column: "leagueid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "fixtures");

            migrationBuilder.DropTable(
                name: "players");

            migrationBuilder.DropTable(
                name: "scoringsystem");

            migrationBuilder.DropTable(
                name: "positions");

            migrationBuilder.DropTable(
                name: "teams");

            migrationBuilder.DropTable(
                name: "leagues");

            migrationBuilder.DropTable(
                name: "sports");
        }
    }
}
