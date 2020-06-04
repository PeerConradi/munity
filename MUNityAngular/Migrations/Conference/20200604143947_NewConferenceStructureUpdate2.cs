using Microsoft.EntityFrameworkCore.Migrations;

namespace MUNityAngular.Migrations.Conference
{
    public partial class NewConferenceStructureUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Committees_Conferences_ConferenceId",
                table: "Committees");

            migrationBuilder.AddForeignKey(
                name: "FK_Committees_Conferences_ConferenceId",
                table: "Committees",
                column: "ConferenceId",
                principalTable: "Conferences",
                principalColumn: "ConferenceId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Committees_Conferences_ConferenceId",
                table: "Committees");

            migrationBuilder.AddForeignKey(
                name: "FK_Committees_Conferences_ConferenceId",
                table: "Committees",
                column: "ConferenceId",
                principalTable: "Conferences",
                principalColumn: "ConferenceId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
