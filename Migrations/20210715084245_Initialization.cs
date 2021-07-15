using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace OpenFIS.Migrations
{
    public partial class Initialization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "athletes",
                columns: table => new
                {
                    fis_code = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    year = table.Column<int>(type: "integer", nullable: true),
                    nation = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_athletes", x => x.fis_code);
                });

            migrationBuilder.CreateTable(
                name: "competition_places",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    city = table.Column<string>(type: "text", nullable: false),
                    country = table.Column<string>(type: "text", nullable: false),
                    construction_point = table.Column<int>(type: "integer", nullable: true),
                    hill_size = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_competition_places", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "competition_results",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    competition_gender_type = table.Column<int>(type: "integer", nullable: false),
                    competition_type = table.Column<int>(type: "integer", nullable: false),
                    competition_place_id = table.Column<int>(type: "integer", nullable: true),
                    competition_date = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_competition_results", x => x.id);
                    table.ForeignKey(
                        name: "fk_competition_results_competition_places_competition_place_id",
                        column: x => x.competition_place_id,
                        principalTable: "competition_places",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "competitor_results",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    rank = table.Column<int>(type: "integer", nullable: false),
                    bib = table.Column<int>(type: "integer", nullable: false),
                    athlete_fis_code = table.Column<int>(type: "integer", nullable: true),
                    total_points = table.Column<float>(type: "real", nullable: true),
                    competition_result_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_competitor_results", x => x.id);
                    table.ForeignKey(
                        name: "fk_competitor_results_athletes_athlete_fis_code",
                        column: x => x.athlete_fis_code,
                        principalTable: "athletes",
                        principalColumn: "fis_code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_competitor_results_competition_results_competition_result_id",
                        column: x => x.competition_result_id,
                        principalTable: "competition_results",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "athlete_results",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    competition_type = table.Column<int>(type: "integer", nullable: false),
                    competition_place_id = table.Column<int>(type: "integer", nullable: true),
                    competition_date = table.Column<string>(type: "text", nullable: true),
                    athlete_result_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_athlete_results", x => x.id);
                    table.ForeignKey(
                        name: "fk_athlete_results_competition_places_competition_place_id",
                        column: x => x.competition_place_id,
                        principalTable: "competition_places",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_athlete_results_competitor_results_athlete_result_id",
                        column: x => x.athlete_result_id,
                        principalTable: "competitor_results",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "competitor_result_jumps",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    length = table.Column<float>(type: "real", nullable: true),
                    point = table.Column<float>(type: "real", nullable: true),
                    competitor_result_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_competitor_result_jumps", x => x.id);
                    table.ForeignKey(
                        name: "fk_competitor_result_jumps_competitor_results_competitor_resul",
                        column: x => x.competitor_result_id,
                        principalTable: "competitor_results",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_athlete_results_athlete_result_id",
                table: "athlete_results",
                column: "athlete_result_id");

            migrationBuilder.CreateIndex(
                name: "ix_athlete_results_competition_place_id",
                table: "athlete_results",
                column: "competition_place_id");

            migrationBuilder.CreateIndex(
                name: "ix_competition_results_competition_place_id",
                table: "competition_results",
                column: "competition_place_id");

            migrationBuilder.CreateIndex(
                name: "ix_competitor_result_jumps_competitor_result_id",
                table: "competitor_result_jumps",
                column: "competitor_result_id");

            migrationBuilder.CreateIndex(
                name: "ix_competitor_results_athlete_fis_code",
                table: "competitor_results",
                column: "athlete_fis_code");

            migrationBuilder.CreateIndex(
                name: "ix_competitor_results_competition_result_id",
                table: "competitor_results",
                column: "competition_result_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "athlete_results");

            migrationBuilder.DropTable(
                name: "competitor_result_jumps");

            migrationBuilder.DropTable(
                name: "competitor_results");

            migrationBuilder.DropTable(
                name: "athletes");

            migrationBuilder.DropTable(
                name: "competition_results");

            migrationBuilder.DropTable(
                name: "competition_places");
        }
    }
}
