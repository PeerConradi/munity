using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MUNityCore.Migrations.Munity
{
    public partial class InitBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResolutionAuths",
                columns: table => new
                {
                    ResolutionId = table.Column<string>(nullable: false),
                    CreationUserId = table.Column<int>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    LastChangeTime = table.Column<DateTime>(nullable: false),
                    AllowPublicRead = table.Column<bool>(nullable: false),
                    AllowPublicEdit = table.Column<bool>(nullable: false),
                    AllowConferenceRead = table.Column<bool>(nullable: false),
                    AllowCommitteeRead = table.Column<bool>(nullable: false),
                    PublicShortKey = table.Column<string>(nullable: true),
                    ConferenceId = table.Column<string>(nullable: true),
                    CommitteeId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResolutionAuths", x => x.ResolutionId);
                });

            migrationBuilder.CreateTable(
                name: "ResolutionUsers",
                columns: table => new
                {
                    ResolutionUserId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CoreUserId = table.Column<int>(nullable: false),
                    Username = table.Column<string>(maxLength: 100, nullable: true),
                    ForeName = table.Column<string>(maxLength: 150, nullable: true),
                    LastName = table.Column<string>(maxLength: 150, nullable: true),
                    CanRead = table.Column<bool>(nullable: false),
                    CanWrite = table.Column<bool>(nullable: false),
                    CanAddUsers = table.Column<bool>(nullable: false),
                    AuthResolutionId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResolutionUsers", x => x.ResolutionUserId);
                    table.ForeignKey(
                        name: "FK_ResolutionUsers_ResolutionAuths_AuthResolutionId",
                        column: x => x.AuthResolutionId,
                        principalTable: "ResolutionAuths",
                        principalColumn: "ResolutionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SimulationUser",
                columns: table => new
                {
                    SimulationUserId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DisplayName = table.Column<string>(nullable: true),
                    RoleSimulationRoleId = table.Column<int>(nullable: true),
                    CanCreateRole = table.Column<bool>(nullable: false),
                    CanSelectRole = table.Column<bool>(nullable: false),
                    CanEditResolution = table.Column<bool>(nullable: false),
                    CanEditListOfSpeakers = table.Column<bool>(nullable: false),
                    SimulationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimulationUser", x => x.SimulationUserId);
                });

            migrationBuilder.CreateTable(
                name: "AllChatMessage",
                columns: table => new
                {
                    AllChatMessageId = table.Column<string>(nullable: false),
                    AuthorId = table.Column<int>(nullable: false),
                    AuthorName = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    SimulationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllChatMessage", x => x.AllChatMessageId);
                });

            migrationBuilder.CreateTable(
                name: "SimSimRequestModel",
                columns: table => new
                {
                    SimSimRequestModelId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserToken = table.Column<string>(nullable: true),
                    RequestType = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    RequestTime = table.Column<DateTime>(nullable: false),
                    SimulationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimSimRequestModel", x => x.SimSimRequestModelId);
                });

            migrationBuilder.CreateTable(
                name: "SimulationRoles",
                columns: table => new
                {
                    SimulationRoleId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 250, nullable: true),
                    RoleType = table.Column<int>(nullable: false),
                    Iso = table.Column<string>(maxLength: 2, nullable: true),
                    RoleKey = table.Column<string>(maxLength: 32, nullable: true),
                    RoleMaxSlots = table.Column<int>(nullable: false),
                    SimulationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimulationRoles", x => x.SimulationRoleId);
                });

            migrationBuilder.CreateTable(
                name: "SpeakerlistModel",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PublicId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Speakertime = table.Column<TimeSpan>(nullable: false),
                    Questiontime = table.Column<TimeSpan>(nullable: false),
                    RemainingSpeakerTime = table.Column<TimeSpan>(nullable: false),
                    RemainingQuestionTime = table.Column<TimeSpan>(nullable: false),
                    CurrentSpeakerId = table.Column<string>(nullable: true),
                    CurrentQuestionId = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ListClosed = table.Column<bool>(nullable: false),
                    QuestionsClosed = table.Column<bool>(nullable: false),
                    LowTimeMark = table.Column<TimeSpan>(nullable: false),
                    SpeakerLowTime = table.Column<bool>(nullable: false),
                    QuestionLowTime = table.Column<bool>(nullable: false),
                    SpeakerTimeout = table.Column<bool>(nullable: false),
                    QuestionTimeout = table.Column<bool>(nullable: false),
                    ConferenceId = table.Column<string>(nullable: true),
                    CommitteeId = table.Column<string>(nullable: true),
                    StartSpeakerTime = table.Column<DateTime>(nullable: false),
                    StartQuestionTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeakerlistModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Simulations",
                columns: table => new
                {
                    SimulationId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    LobbyMode = table.Column<int>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    CanJoin = table.Column<bool>(nullable: false),
                    ListOfSpeakersId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Simulations", x => x.SimulationId);
                    table.ForeignKey(
                        name: "FK_Simulations_SpeakerlistModel_ListOfSpeakersId",
                        column: x => x.ListOfSpeakersId,
                        principalTable: "SpeakerlistModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Speaker",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Iso = table.Column<string>(nullable: true),
                    SpeakerlistModelId = table.Column<string>(nullable: true),
                    SpeakerlistModelId1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Speaker", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Speaker_SpeakerlistModel_SpeakerlistModelId",
                        column: x => x.SpeakerlistModelId,
                        principalTable: "SpeakerlistModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Speaker_SpeakerlistModel_SpeakerlistModelId1",
                        column: x => x.SpeakerlistModelId1,
                        principalTable: "SpeakerlistModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllChatMessage_SimulationId",
                table: "AllChatMessage",
                column: "SimulationId");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionUsers_AuthResolutionId",
                table: "ResolutionUsers",
                column: "AuthResolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_SimSimRequestModel_SimulationId",
                table: "SimSimRequestModel",
                column: "SimulationId");

            migrationBuilder.CreateIndex(
                name: "IX_SimulationRoles_SimulationId",
                table: "SimulationRoles",
                column: "SimulationId");

            migrationBuilder.CreateIndex(
                name: "IX_Simulations_ListOfSpeakersId",
                table: "Simulations",
                column: "ListOfSpeakersId");

            migrationBuilder.CreateIndex(
                name: "IX_SimulationUser_RoleSimulationRoleId",
                table: "SimulationUser",
                column: "RoleSimulationRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SimulationUser_SimulationId",
                table: "SimulationUser",
                column: "SimulationId");

            migrationBuilder.CreateIndex(
                name: "IX_Speaker_SpeakerlistModelId",
                table: "Speaker",
                column: "SpeakerlistModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Speaker_SpeakerlistModelId1",
                table: "Speaker",
                column: "SpeakerlistModelId1");

            migrationBuilder.CreateIndex(
                name: "IX_SpeakerlistModel_CurrentQuestionId",
                table: "SpeakerlistModel",
                column: "CurrentQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SpeakerlistModel_CurrentSpeakerId",
                table: "SpeakerlistModel",
                column: "CurrentSpeakerId");

            migrationBuilder.AddForeignKey(
                name: "FK_SimulationUser_Simulations_SimulationId",
                table: "SimulationUser",
                column: "SimulationId",
                principalTable: "Simulations",
                principalColumn: "SimulationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SimulationUser_SimulationRoles_RoleSimulationRoleId",
                table: "SimulationUser",
                column: "RoleSimulationRoleId",
                principalTable: "SimulationRoles",
                principalColumn: "SimulationRoleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AllChatMessage_Simulations_SimulationId",
                table: "AllChatMessage",
                column: "SimulationId",
                principalTable: "Simulations",
                principalColumn: "SimulationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SimSimRequestModel_Simulations_SimulationId",
                table: "SimSimRequestModel",
                column: "SimulationId",
                principalTable: "Simulations",
                principalColumn: "SimulationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SimulationRoles_Simulations_SimulationId",
                table: "SimulationRoles",
                column: "SimulationId",
                principalTable: "Simulations",
                principalColumn: "SimulationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SpeakerlistModel_Speaker_CurrentQuestionId",
                table: "SpeakerlistModel",
                column: "CurrentQuestionId",
                principalTable: "Speaker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SpeakerlistModel_Speaker_CurrentSpeakerId",
                table: "SpeakerlistModel",
                column: "CurrentSpeakerId",
                principalTable: "Speaker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Speaker_SpeakerlistModel_SpeakerlistModelId",
                table: "Speaker");

            migrationBuilder.DropForeignKey(
                name: "FK_Speaker_SpeakerlistModel_SpeakerlistModelId1",
                table: "Speaker");

            migrationBuilder.DropTable(
                name: "AllChatMessage");

            migrationBuilder.DropTable(
                name: "ResolutionUsers");

            migrationBuilder.DropTable(
                name: "SimSimRequestModel");

            migrationBuilder.DropTable(
                name: "SimulationUser");

            migrationBuilder.DropTable(
                name: "ResolutionAuths");

            migrationBuilder.DropTable(
                name: "SimulationRoles");

            migrationBuilder.DropTable(
                name: "Simulations");

            migrationBuilder.DropTable(
                name: "SpeakerlistModel");

            migrationBuilder.DropTable(
                name: "Speaker");
        }
    }
}
