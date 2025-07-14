using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mission.Entities.Migrations
{
    /// <inheritdoc />
    public partial class AddMission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Missions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mission_title = table.Column<string>(type: "text", nullable: false),
                    mission_description = table.Column<string>(type: "text", nullable: false),
                    mission_organisation_name = table.Column<string>(type: "text", nullable: false),
                    mission_organisation_detail = table.Column<string>(type: "text", nullable: false),
                    country_id = table.Column<int>(type: "integer", nullable: false),
                    city_id = table.Column<int>(type: "integer", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    mission_type = table.Column<string>(type: "text", nullable: false),
                    totalsheets = table.Column<int>(type: "integer", nullable: false),
                    mission_theme_id = table.Column<int>(type: "integer", nullable: false),
                    mission_skill_id = table.Column<string>(type: "text", nullable: false),
                    mission_images = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Missions", x => x.id);
                    table.ForeignKey(
                        name: "FK_Missions_City_country_id",
                        column: x => x.country_id,
                        principalTable: "City",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Missions_Country_city_id",
                        column: x => x.city_id,
                        principalTable: "Country",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Missions_MissionTheme_mission_theme_id",
                        column: x => x.mission_theme_id,
                        principalTable: "MissionTheme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Missions_city_id",
                table: "Missions",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "IX_Missions_country_id",
                table: "Missions",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_Missions_mission_theme_id",
                table: "Missions",
                column: "mission_theme_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Missions");
        }
    }
}
