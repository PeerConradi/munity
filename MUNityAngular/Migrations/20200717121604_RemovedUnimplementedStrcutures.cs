using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MUNityAngular.Migrations
{
    public partial class RemovedUnimplementedStrcutures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResolutionUsers_ResolutionAuths_ResolutionAuthResolutionId",
                table: "ResolutionUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ResolutionUsers_ResolutionAuths_ResolutionAuthResolutionId1",
                table: "ResolutionUsers");

            migrationBuilder.DropTable(
                name: "AuthKey");

            migrationBuilder.DropTable(
                name: "CommitteeTopic");

            migrationBuilder.DropTable(
                name: "MediaTag");

            migrationBuilder.DropTable(
                name: "OrganisationMember");

            migrationBuilder.DropTable(
                name: "Committee");

            migrationBuilder.DropTable(
                name: "MediaImages");

            migrationBuilder.DropTable(
                name: "OrganisationRole");

            migrationBuilder.DropTable(
                name: "Galleries");

            migrationBuilder.DropTable(
                name: "Conference");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Organisation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResolutionUsers",
                table: "ResolutionUsers");

            migrationBuilder.RenameTable(
                name: "ResolutionUsers",
                newName: "ResolutionUser");

            migrationBuilder.RenameIndex(
                name: "IX_ResolutionUsers_ResolutionAuthResolutionId1",
                table: "ResolutionUser",
                newName: "IX_ResolutionUser_ResolutionAuthResolutionId1");

            migrationBuilder.RenameIndex(
                name: "IX_ResolutionUsers_ResolutionAuthResolutionId",
                table: "ResolutionUser",
                newName: "IX_ResolutionUser_ResolutionAuthResolutionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResolutionUser",
                table: "ResolutionUser",
                column: "ResolutionUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResolutionUser_ResolutionAuths_ResolutionAuthResolutionId",
                table: "ResolutionUser",
                column: "ResolutionAuthResolutionId",
                principalTable: "ResolutionAuths",
                principalColumn: "ResolutionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResolutionUser_ResolutionAuths_ResolutionAuthResolutionId1",
                table: "ResolutionUser",
                column: "ResolutionAuthResolutionId1",
                principalTable: "ResolutionAuths",
                principalColumn: "ResolutionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResolutionUser_ResolutionAuths_ResolutionAuthResolutionId",
                table: "ResolutionUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ResolutionUser_ResolutionAuths_ResolutionAuthResolutionId1",
                table: "ResolutionUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResolutionUser",
                table: "ResolutionUser");

            migrationBuilder.RenameTable(
                name: "ResolutionUser",
                newName: "ResolutionUsers");

            migrationBuilder.RenameIndex(
                name: "IX_ResolutionUser_ResolutionAuthResolutionId1",
                table: "ResolutionUsers",
                newName: "IX_ResolutionUsers_ResolutionAuthResolutionId1");

            migrationBuilder.RenameIndex(
                name: "IX_ResolutionUser_ResolutionAuthResolutionId",
                table: "ResolutionUsers",
                newName: "IX_ResolutionUsers_ResolutionAuthResolutionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResolutionUsers",
                table: "ResolutionUsers",
                column: "ResolutionUserId");

            migrationBuilder.CreateTable(
                name: "Organisation",
                columns: table => new
                {
                    OrganisationId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    OrganisationAbbreviation = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    OrganisationName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisation", x => x.OrganisationId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Birthday = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    City = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Forename = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Gender = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Housenumber = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    LastOnline = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Lastname = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Mail = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Password = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ProfileImageName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Salt = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Street = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Title = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Username = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Zipcode = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "OrganisationRole",
                columns: table => new
                {
                    OrganisationRoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CanCreateProject = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    OrganisationId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    RoleName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganisationRole", x => x.OrganisationRoleId);
                    table.ForeignKey(
                        name: "FK_OrganisationRole_Organisation_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisation",
                        principalColumn: "OrganisationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    ProjectId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    ProjectAbbreviation = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ProjectName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ProjectOrganisationOrganisationId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.ProjectId);
                    table.ForeignKey(
                        name: "FK_Project_Organisation_ProjectOrganisationOrganisationId",
                        column: x => x.ProjectOrganisationOrganisationId,
                        principalTable: "Organisation",
                        principalColumn: "OrganisationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuthKey",
                columns: table => new
                {
                    AuthKeyValue = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    GenerationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    GenerationDevice = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    GenerationIp = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthKey", x => x.AuthKeyValue);
                    table.ForeignKey(
                        name: "FK_AuthKey_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrganisationMember",
                columns: table => new
                {
                    OrganisationMemberId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrganisationId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    RoleOrganisationRoleId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganisationMember", x => x.OrganisationMemberId);
                    table.ForeignKey(
                        name: "FK_OrganisationMember_Organisation_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisation",
                        principalColumn: "OrganisationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrganisationMember_OrganisationRole_RoleOrganisationRoleId",
                        column: x => x.RoleOrganisationRoleId,
                        principalTable: "OrganisationRole",
                        principalColumn: "OrganisationRoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrganisationMember_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Conference",
                columns: table => new
                {
                    ConferenceId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    Abbreviation = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ConferenceProjectProjectId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreationUserUserId = table.Column<int>(type: "int", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FullName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conference", x => x.ConferenceId);
                    table.ForeignKey(
                        name: "FK_Conference_Project_ConferenceProjectProjectId",
                        column: x => x.ConferenceProjectProjectId,
                        principalTable: "Project",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conference_User_CreationUserUserId",
                        column: x => x.CreationUserUserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Committee",
                columns: table => new
                {
                    CommitteeId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    Abbreviation = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Article = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ConferenceId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    FullName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ResolutlyCommitteeCommitteeId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Committee", x => x.CommitteeId);
                    table.ForeignKey(
                        name: "FK_Committee_Conference_ConferenceId",
                        column: x => x.ConferenceId,
                        principalTable: "Conference",
                        principalColumn: "ConferenceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Committee_Committee_ResolutlyCommitteeCommitteeId",
                        column: x => x.ResolutlyCommitteeCommitteeId,
                        principalTable: "Committee",
                        principalColumn: "CommitteeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Galleries",
                columns: table => new
                {
                    GalleryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConferenceId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Galleries", x => x.GalleryId);
                    table.ForeignKey(
                        name: "FK_Galleries_Conference_ConferenceId",
                        column: x => x.ConferenceId,
                        principalTable: "Conference",
                        principalColumn: "ConferenceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CommitteeTopic",
                columns: table => new
                {
                    CommitteeTopicId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CommitteeId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    TopicCode = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    TopicDescription = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    TopicFullName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    TopicName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommitteeTopic", x => x.CommitteeTopicId);
                    table.ForeignKey(
                        name: "FK_CommitteeTopic_Committee_CommitteeId",
                        column: x => x.CommitteeId,
                        principalTable: "Committee",
                        principalColumn: "CommitteeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MediaImages",
                columns: table => new
                {
                    MediaImageId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    Description = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    GalleryId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    OwnerUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaImages", x => x.MediaImageId);
                    table.ForeignKey(
                        name: "FK_MediaImages_Galleries_GalleryId",
                        column: x => x.GalleryId,
                        principalTable: "Galleries",
                        principalColumn: "GalleryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MediaImages_User_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MediaTag",
                columns: table => new
                {
                    MediaTagId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MediaImageId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    TagName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaTag", x => x.MediaTagId);
                    table.ForeignKey(
                        name: "FK_MediaTag_MediaImages_MediaImageId",
                        column: x => x.MediaImageId,
                        principalTable: "MediaImages",
                        principalColumn: "MediaImageId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthKey_UserId",
                table: "AuthKey",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Committee_ConferenceId",
                table: "Committee",
                column: "ConferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Committee_ResolutlyCommitteeCommitteeId",
                table: "Committee",
                column: "ResolutlyCommitteeCommitteeId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitteeTopic_CommitteeId",
                table: "CommitteeTopic",
                column: "CommitteeId");

            migrationBuilder.CreateIndex(
                name: "IX_Conference_ConferenceProjectProjectId",
                table: "Conference",
                column: "ConferenceProjectProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Conference_CreationUserUserId",
                table: "Conference",
                column: "CreationUserUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Galleries_ConferenceId",
                table: "Galleries",
                column: "ConferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaImages_GalleryId",
                table: "MediaImages",
                column: "GalleryId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaImages_OwnerUserId",
                table: "MediaImages",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaTag_MediaImageId",
                table: "MediaTag",
                column: "MediaImageId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganisationMember_OrganisationId",
                table: "OrganisationMember",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganisationMember_RoleOrganisationRoleId",
                table: "OrganisationMember",
                column: "RoleOrganisationRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganisationMember_UserId",
                table: "OrganisationMember",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganisationRole_OrganisationId",
                table: "OrganisationRole",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_ProjectOrganisationOrganisationId",
                table: "Project",
                column: "ProjectOrganisationOrganisationId");

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
    }
}
