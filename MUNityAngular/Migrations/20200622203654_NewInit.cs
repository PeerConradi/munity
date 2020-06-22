using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MUNityAngular.Migrations
{
    public partial class NewInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organisation",
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

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Mail = table.Column<string>(nullable: true),
                    Salt = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Forename = table.Column<string>(nullable: true),
                    Lastname = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    Birthday = table.Column<DateTime>(nullable: false),
                    Street = table.Column<string>(nullable: true),
                    Zipcode = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Housenumber = table.Column<string>(nullable: true),
                    ProfileImageName = table.Column<string>(nullable: true),
                    RegistrationDate = table.Column<DateTime>(nullable: false),
                    LastOnline = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "OrganisationRole",
                columns: table => new
                {
                    OrganisationRoleId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(nullable: true),
                    OrganisationId = table.Column<string>(nullable: true),
                    CanCreateProject = table.Column<bool>(nullable: false)
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
                    ProjectId = table.Column<string>(nullable: false),
                    ProjectName = table.Column<string>(nullable: true),
                    ProjectAbbreviation = table.Column<string>(nullable: true),
                    ProjectOrganisationOrganisationId = table.Column<string>(nullable: true)
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
                    AuthKeyValue = table.Column<string>(nullable: false),
                    UserId = table.Column<int>(nullable: true),
                    GenerationDate = table.Column<DateTime>(nullable: false),
                    GenerationIp = table.Column<string>(nullable: true),
                    GenerationDevice = table.Column<string>(nullable: true)
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
                name: "Resolutions",
                columns: table => new
                {
                    ResolutionId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    OnlineCode = table.Column<string>(nullable: true),
                    PublicRead = table.Column<bool>(nullable: false),
                    PublicWrite = table.Column<bool>(nullable: false),
                    PublicAmendment = table.Column<bool>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    LastChangedDate = table.Column<DateTime>(nullable: false),
                    CreationUserUserId = table.Column<int>(nullable: true)
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
                name: "OrganisationMember",
                columns: table => new
                {
                    OrganisationMemberId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: true),
                    OrganisationId = table.Column<string>(nullable: true),
                    RoleOrganisationRoleId = table.Column<int>(nullable: true)
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
                    ConferenceId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    Abbreviation = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    CreationUserUserId = table.Column<int>(nullable: true),
                    ConferenceProjectProjectId = table.Column<string>(nullable: true)
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
                name: "ResolutionUsers",
                columns: table => new
                {
                    ResolutionUserId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ResolutionId = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    canRead = table.Column<bool>(nullable: false),
                    CanEdit = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResolutionUsers", x => x.ResolutionUserId);
                    table.ForeignKey(
                        name: "FK_ResolutionUsers_Resolutions_ResolutionId",
                        column: x => x.ResolutionId,
                        principalTable: "Resolutions",
                        principalColumn: "ResolutionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResolutionUsers_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Committee",
                columns: table => new
                {
                    CommitteeId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    Abbreviation = table.Column<string>(nullable: true),
                    Article = table.Column<string>(nullable: true),
                    ResolutlyCommitteeCommitteeId = table.Column<string>(nullable: true),
                    ConferenceId = table.Column<string>(nullable: true)
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
                    GalleryId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    ConferenceId = table.Column<string>(nullable: true)
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
                    CommitteeTopicId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TopicName = table.Column<string>(nullable: true),
                    TopicFullName = table.Column<string>(nullable: true),
                    TopicDescription = table.Column<string>(nullable: true),
                    TopicCode = table.Column<string>(nullable: true),
                    CommitteeId = table.Column<string>(nullable: true)
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
                name: "ConferenceResolutions",
                columns: table => new
                {
                    ResolutionConferenceId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CommitteeId = table.Column<string>(nullable: true),
                    ConferenceId = table.Column<string>(nullable: true),
                    ResolutionId = table.Column<string>(nullable: true)
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

            migrationBuilder.CreateTable(
                name: "MediaImages",
                columns: table => new
                {
                    MediaImageId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    OwnerUserId = table.Column<int>(nullable: true),
                    GalleryId = table.Column<int>(nullable: true)
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
                    MediaTagId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TagName = table.Column<string>(nullable: true),
                    MediaImageId = table.Column<string>(nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_Resolutions_CreationUserUserId",
                table: "Resolutions",
                column: "CreationUserUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionUsers_ResolutionId",
                table: "ResolutionUsers",
                column: "ResolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionUsers_UserId",
                table: "ResolutionUsers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthKey");

            migrationBuilder.DropTable(
                name: "CommitteeTopic");

            migrationBuilder.DropTable(
                name: "ConferenceResolutions");

            migrationBuilder.DropTable(
                name: "MediaTag");

            migrationBuilder.DropTable(
                name: "OrganisationMember");

            migrationBuilder.DropTable(
                name: "ResolutionUsers");

            migrationBuilder.DropTable(
                name: "Committee");

            migrationBuilder.DropTable(
                name: "MediaImages");

            migrationBuilder.DropTable(
                name: "OrganisationRole");

            migrationBuilder.DropTable(
                name: "Resolutions");

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
        }
    }
}
