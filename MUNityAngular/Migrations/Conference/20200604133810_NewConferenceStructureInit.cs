using Microsoft.EntityFrameworkCore.Migrations;

namespace MUNityAngular.Migrations.Conference
{
    public partial class NewConferenceStructureInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organisations",
                columns: table => new
                {
                    OrganisationId = table.Column<string>(nullable: false),
                    OrganisationName = table.Column<string>(nullable: true),
                    OrganisationAbbreviation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisation", x => x.OrganisationId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Organisations");
        }
    }
}
