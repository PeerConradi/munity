using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MUNityCore.Migrations
{
    public partial class AddedListOfSpeakersLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.CreateTable(
                name: "ListOfSpeakersLog",
                columns: table => new
                {
                    ListOfSpeakersLogId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SpeakerlistListOfSpeakersId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    SpeakerIso = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    SpeakerName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    PermitedSpeakingSeconds = table.Column<long>(type: "bigint", nullable: false),
                    PermitedQuestionsSeconds = table.Column<long>(type: "bigint", nullable: false),
                    UsedSpeakerSeconds = table.Column<long>(type: "bigint", nullable: false),
                    UsedQuestionSeconds = table.Column<long>(type: "bigint", nullable: false),
                    TimesOnSpeakerlist = table.Column<int>(type: "int", nullable: false),
                    TimesOnQuestions = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListOfSpeakersLog", x => x.ListOfSpeakersLogId);
                    table.ForeignKey(
                        name: "FK_ListOfSpeakersLog_ListOfSpeakers_SpeakerlistListOfSpeakersId",
                        column: x => x.SpeakerlistListOfSpeakersId,
                        principalTable: "ListOfSpeakers",
                        principalColumn: "ListOfSpeakersId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListOfSpeakersLog_SpeakerlistListOfSpeakersId",
                table: "ListOfSpeakersLog",
                column: "SpeakerlistListOfSpeakersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListOfSpeakersLog");

        }
    }
}
