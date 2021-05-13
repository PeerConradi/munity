using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MUNityCore.Migrations
{
    public partial class AddedColumnsListOfSpeakersLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<int>(
                name: "TimesQuestioning",
                table: "ListOfSpeakersLog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimesSpeaking",
                table: "ListOfSpeakersLog",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimesQuestioning",
                table: "ListOfSpeakersLog");

            migrationBuilder.DropColumn(
                name: "TimesSpeaking",
                table: "ListOfSpeakersLog");

     
        }
    }
}
