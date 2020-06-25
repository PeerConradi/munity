using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MUNityAngular.Migrations
{
    public partial class AddedResolutionAuth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResolutionUsers_Resolutions_ResolutionId",
                table: "ResolutionUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ResolutionUsers_User_UserId",
                table: "ResolutionUsers");

            migrationBuilder.DropTable(
                name: "ConferenceResolutions");

            migrationBuilder.DropTable(
                name: "Resolutions");

            migrationBuilder.DropIndex(
                name: "IX_ResolutionUsers_ResolutionId",
                table: "ResolutionUsers");

            migrationBuilder.DropIndex(
                name: "IX_ResolutionUsers_UserId",
                table: "ResolutionUsers");

            migrationBuilder.DropColumn(
                name: "AddedDate",
                table: "ResolutionUsers");

            migrationBuilder.DropColumn(
                name: "CanEdit",
                table: "ResolutionUsers");

            migrationBuilder.DropColumn(
                name: "ResolutionId",
                table: "ResolutionUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ResolutionUsers");

            migrationBuilder.DropColumn(
                name: "canRead",
                table: "ResolutionUsers");

            migrationBuilder.AddColumn<int>(
                name: "CoreUserId",
                table: "ResolutionUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ForeName",
                table: "ResolutionUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "ResolutionUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResolutionAuthResolutionId",
                table: "ResolutionUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResolutionAuthResolutionId1",
                table: "ResolutionUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "ResolutionUsers",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionUsers_ResolutionAuthResolutionId",
                table: "ResolutionUsers",
                column: "ResolutionAuthResolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionUsers_ResolutionAuthResolutionId1",
                table: "ResolutionUsers",
                column: "ResolutionAuthResolutionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ResolutionUsers_ResolutionAuths_ResolutionAuthResolutionId",
                table: "ResolutionUsers",
                column: "ResolutionAuthResolutionId",
                principalTable: "ResolutionAuths",
                principalColumn: "ResolutionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResolutionUsers_ResolutionAuths_ResolutionAuthResolutionId1",
                table: "ResolutionUsers",
                column: "ResolutionAuthResolutionId1",
                principalTable: "ResolutionAuths",
                principalColumn: "ResolutionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResolutionUsers_ResolutionAuths_ResolutionAuthResolutionId",
                table: "ResolutionUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ResolutionUsers_ResolutionAuths_ResolutionAuthResolutionId1",
                table: "ResolutionUsers");

            migrationBuilder.DropTable(
                name: "ResolutionAuths");

            migrationBuilder.DropIndex(
                name: "IX_ResolutionUsers_ResolutionAuthResolutionId",
                table: "ResolutionUsers");

            migrationBuilder.DropIndex(
                name: "IX_ResolutionUsers_ResolutionAuthResolutionId1",
                table: "ResolutionUsers");

            migrationBuilder.DropColumn(
                name: "CoreUserId",
                table: "ResolutionUsers");

            migrationBuilder.DropColumn(
                name: "ForeName",
                table: "ResolutionUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "ResolutionUsers");

            migrationBuilder.DropColumn(
                name: "ResolutionAuthResolutionId",
                table: "ResolutionUsers");

            migrationBuilder.DropColumn(
                name: "ResolutionAuthResolutionId1",
                table: "ResolutionUsers");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "ResolutionUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedDate",
                table: "ResolutionUsers",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "CanEdit",
                table: "ResolutionUsers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ResolutionId",
                table: "ResolutionUsers",
                type: "varchar(95) CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ResolutionUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "canRead",
                table: "ResolutionUsers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Resolutions",
                columns: table => new
                {
                    ResolutionId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreationUserUserId = table.Column<int>(type: "int", nullable: true),
                    LastChangedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    OnlineCode = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    PublicAmendment = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PublicRead = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PublicWrite = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resolutions", x => x.ResolutionId);
                    table.ForeignKey(
                        name: "FK_Resolutions_User_CreationUserUserId",
                        column: x => x.CreationUserUserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConferenceResolutions",
                columns: table => new
                {
                    ResolutionConferenceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CommitteeId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    ConferenceId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    ResolutionId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConferenceResolutions", x => x.ResolutionConferenceId);
                    table.ForeignKey(
                        name: "FK_ConferenceResolutions_Committee_CommitteeId",
                        column: x => x.CommitteeId,
                        principalTable: "Committee",
                        principalColumn: "CommitteeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConferenceResolutions_Conference_ConferenceId",
                        column: x => x.ConferenceId,
                        principalTable: "Conference",
                        principalColumn: "ConferenceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConferenceResolutions_Resolutions_ResolutionId",
                        column: x => x.ResolutionId,
                        principalTable: "Resolutions",
                        principalColumn: "ResolutionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionUsers_ResolutionId",
                table: "ResolutionUsers",
                column: "ResolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionUsers_UserId",
                table: "ResolutionUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ConferenceResolutions_CommitteeId",
                table: "ConferenceResolutions",
                column: "CommitteeId");

            migrationBuilder.CreateIndex(
                name: "IX_ConferenceResolutions_ConferenceId",
                table: "ConferenceResolutions",
                column: "ConferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_ConferenceResolutions_ResolutionId",
                table: "ConferenceResolutions",
                column: "ResolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_Resolutions_CreationUserUserId",
                table: "Resolutions",
                column: "CreationUserUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResolutionUsers_Resolutions_ResolutionId",
                table: "ResolutionUsers",
                column: "ResolutionId",
                principalTable: "Resolutions",
                principalColumn: "ResolutionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResolutionUsers_User_UserId",
                table: "ResolutionUsers",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
