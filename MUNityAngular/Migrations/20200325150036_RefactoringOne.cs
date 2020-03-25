using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MUNityAngular.Migrations
{
    public partial class RefactoringOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Abbreviation",
                table: "Conferences",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Conferences",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreationUserUserId",
                table: "Conferences",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Conferences",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SecretaryGeneralName",
                table: "Conferences",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecretaryGeneralTitle",
                table: "Conferences",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Conferences",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Abbreviation",
                table: "Committees",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Article",
                table: "Committees",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResolutlyCommitteeCommitteeId",
                table: "Committees",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CommitteeStatuses",
                columns: table => new
                {
                    CommitteeStatusId = table.Column<string>(nullable: false),
                    CommitteeId = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    AgendaItem = table.Column<string>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommitteeStatuses", x => x.CommitteeStatusId);
                    table.ForeignKey(
                        name: "FK_CommitteeStatuses_Committees_CommitteeId",
                        column: x => x.CommitteeId,
                        principalTable: "Committees",
                        principalColumn: "CommitteeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Delegations",
                columns: table => new
                {
                    DelegationId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    Abbreviation = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    IconName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Delegations", x => x.DelegationId);
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
                        name: "FK_Galleries_Conferences_ConferenceId",
                        column: x => x.ConferenceId,
                        principalTable: "Conferences",
                        principalColumn: "ConferenceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamRoles",
                columns: table => new
                {
                    TeamRoleId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ParentRoleTeamRoleId = table.Column<int>(nullable: true),
                    MinCount = table.Column<int>(nullable: false),
                    MaxCount = table.Column<int>(nullable: false),
                    ConferenceId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamRoles", x => x.TeamRoleId);
                    table.ForeignKey(
                        name: "FK_TeamRoles_Conferences_ConferenceId",
                        column: x => x.ConferenceId,
                        principalTable: "Conferences",
                        principalColumn: "ConferenceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamRoles_TeamRoles_ParentRoleTeamRoleId",
                        column: x => x.ParentRoleTeamRoleId,
                        principalTable: "TeamRoles",
                        principalColumn: "TeamRoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
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
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "CommitteeDelegations",
                columns: table => new
                {
                    CommitteeDelegationId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CommitteeId = table.Column<string>(nullable: true),
                    DelegationId = table.Column<string>(nullable: true),
                    MinCount = table.Column<int>(nullable: false),
                    MaxCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommitteeDelegations", x => x.CommitteeDelegationId);
                    table.ForeignKey(
                        name: "FK_CommitteeDelegations_Committees_CommitteeId",
                        column: x => x.CommitteeId,
                        principalTable: "Committees",
                        principalColumn: "CommitteeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommitteeDelegations_Delegations_DelegationId",
                        column: x => x.DelegationId,
                        principalTable: "Delegations",
                        principalColumn: "DelegationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    AdminId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: true),
                    PowerRank = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.AdminId);
                    table.ForeignKey(
                        name: "FK_Admins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
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
                        name: "FK_AuthKey_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CommitteeLeader",
                columns: table => new
                {
                    CommitteeLeaderId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CommitteeId = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    RoleName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommitteeLeader", x => x.CommitteeLeaderId);
                    table.ForeignKey(
                        name: "FK_CommitteeLeader_Committees_CommitteeId",
                        column: x => x.CommitteeId,
                        principalTable: "Committees",
                        principalColumn: "CommitteeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommitteeLeader_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConferenceUserAuths",
                columns: table => new
                {
                    ConferenceUserAuthId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConferenceId = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    CanRead = table.Column<bool>(nullable: false),
                    CanEditSettings = table.Column<bool>(nullable: false),
                    CanEditPublicRelations = table.Column<bool>(nullable: false),
                    CanEditGallerie = table.Column<bool>(nullable: false),
                    CanSendMails = table.Column<bool>(nullable: false),
                    CanEditTeam = table.Column<bool>(nullable: false),
                    CanLinkResolutions = table.Column<bool>(nullable: false),
                    CanDeleteConference = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConferenceUserAuths", x => x.ConferenceUserAuthId);
                    table.ForeignKey(
                        name: "FK_ConferenceUserAuths_Conferences_ConferenceId",
                        column: x => x.ConferenceId,
                        principalTable: "Conferences",
                        principalColumn: "ConferenceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConferenceUserAuths_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DelegationUsers",
                columns: table => new
                {
                    DelegationUserId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DelegationId = table.Column<string>(nullable: true),
                    CommitteeId = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    IsLeader = table.Column<bool>(nullable: false),
                    JoinDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DelegationUsers", x => x.DelegationUserId);
                    table.ForeignKey(
                        name: "FK_DelegationUsers_Committees_CommitteeId",
                        column: x => x.CommitteeId,
                        principalTable: "Committees",
                        principalColumn: "CommitteeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DelegationUsers_Delegations_DelegationId",
                        column: x => x.DelegationId,
                        principalTable: "Delegations",
                        principalColumn: "DelegationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DelegationUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
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
                        name: "FK_MediaImages_Users_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalTable: "Users",
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
                        name: "FK_Resolutions_Users_CreationUserUserId",
                        column: x => x.CreationUserUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamUsers",
                columns: table => new
                {
                    TeamUserId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleTeamRoleId = table.Column<int>(nullable: true),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamUsers", x => x.TeamUserId);
                    table.ForeignKey(
                        name: "FK_TeamUsers_TeamRoles_RoleTeamRoleId",
                        column: x => x.RoleTeamRoleId,
                        principalTable: "TeamRoles",
                        principalColumn: "TeamRoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserAuths",
                columns: table => new
                {
                    UserAuthsId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: true),
                    CanCreateConference = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAuths", x => x.UserAuthsId);
                    table.ForeignKey(
                        name: "FK_UserAuths_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
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
                        name: "FK_ConferenceResolutions_Committees_CommitteeId",
                        column: x => x.CommitteeId,
                        principalTable: "Committees",
                        principalColumn: "CommitteeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConferenceResolutions_Conferences_ConferenceId",
                        column: x => x.ConferenceId,
                        principalTable: "Conferences",
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
                        name: "FK_ResolutionUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Conferences_CreationUserUserId",
                table: "Conferences",
                column: "CreationUserUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Committees_ResolutlyCommitteeCommitteeId",
                table: "Committees",
                column: "ResolutlyCommitteeCommitteeId");

            migrationBuilder.CreateIndex(
                name: "IX_Admins_UserId",
                table: "Admins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthKey_UserId",
                table: "AuthKey",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitteeDelegations_CommitteeId",
                table: "CommitteeDelegations",
                column: "CommitteeId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitteeDelegations_DelegationId",
                table: "CommitteeDelegations",
                column: "DelegationId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitteeLeader_CommitteeId",
                table: "CommitteeLeader",
                column: "CommitteeId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitteeLeader_UserId",
                table: "CommitteeLeader",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitteeStatuses_CommitteeId",
                table: "CommitteeStatuses",
                column: "CommitteeId");

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
                name: "IX_ConferenceUserAuths_ConferenceId",
                table: "ConferenceUserAuths",
                column: "ConferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_ConferenceUserAuths_UserId",
                table: "ConferenceUserAuths",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DelegationUsers_CommitteeId",
                table: "DelegationUsers",
                column: "CommitteeId");

            migrationBuilder.CreateIndex(
                name: "IX_DelegationUsers_DelegationId",
                table: "DelegationUsers",
                column: "DelegationId");

            migrationBuilder.CreateIndex(
                name: "IX_DelegationUsers_UserId",
                table: "DelegationUsers",
                column: "UserId");

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

            migrationBuilder.CreateIndex(
                name: "IX_TeamRoles_ConferenceId",
                table: "TeamRoles",
                column: "ConferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamRoles_ParentRoleTeamRoleId",
                table: "TeamRoles",
                column: "ParentRoleTeamRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamUsers_RoleTeamRoleId",
                table: "TeamUsers",
                column: "RoleTeamRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamUsers_UserId",
                table: "TeamUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAuths_UserId",
                table: "UserAuths",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Committees_Committees_ResolutlyCommitteeCommitteeId",
                table: "Committees",
                column: "ResolutlyCommitteeCommitteeId",
                principalTable: "Committees",
                principalColumn: "CommitteeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Conferences_Users_CreationUserUserId",
                table: "Conferences",
                column: "CreationUserUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Committees_Committees_ResolutlyCommitteeCommitteeId",
                table: "Committees");

            migrationBuilder.DropForeignKey(
                name: "FK_Conferences_Users_CreationUserUserId",
                table: "Conferences");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "AuthKey");

            migrationBuilder.DropTable(
                name: "CommitteeDelegations");

            migrationBuilder.DropTable(
                name: "CommitteeLeader");

            migrationBuilder.DropTable(
                name: "CommitteeStatuses");

            migrationBuilder.DropTable(
                name: "ConferenceResolutions");

            migrationBuilder.DropTable(
                name: "ConferenceUserAuths");

            migrationBuilder.DropTable(
                name: "DelegationUsers");

            migrationBuilder.DropTable(
                name: "MediaTag");

            migrationBuilder.DropTable(
                name: "ResolutionUsers");

            migrationBuilder.DropTable(
                name: "TeamUsers");

            migrationBuilder.DropTable(
                name: "UserAuths");

            migrationBuilder.DropTable(
                name: "Delegations");

            migrationBuilder.DropTable(
                name: "MediaImages");

            migrationBuilder.DropTable(
                name: "Resolutions");

            migrationBuilder.DropTable(
                name: "TeamRoles");

            migrationBuilder.DropTable(
                name: "Galleries");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Conferences_CreationUserUserId",
                table: "Conferences");

            migrationBuilder.DropIndex(
                name: "IX_Committees_ResolutlyCommitteeCommitteeId",
                table: "Committees");

            migrationBuilder.DropColumn(
                name: "Abbreviation",
                table: "Conferences");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Conferences");

            migrationBuilder.DropColumn(
                name: "CreationUserUserId",
                table: "Conferences");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Conferences");

            migrationBuilder.DropColumn(
                name: "SecretaryGeneralName",
                table: "Conferences");

            migrationBuilder.DropColumn(
                name: "SecretaryGeneralTitle",
                table: "Conferences");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Conferences");

            migrationBuilder.DropColumn(
                name: "Abbreviation",
                table: "Committees");

            migrationBuilder.DropColumn(
                name: "Article",
                table: "Committees");

            migrationBuilder.DropColumn(
                name: "ResolutlyCommitteeCommitteeId",
                table: "Committees");
        }
    }
}
