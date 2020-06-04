using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MUNityAngular.Migrations.Conference
{
    public partial class NewConferenceStructureUpdate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Delegation",
                columns: table => new
                {
                    DelegationId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Delegation", x => x.DelegationId);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectId = table.Column<string>(nullable: false),
                    ProjectName = table.Column<string>(nullable: true),
                    ProjectAbbreviation = table.Column<string>(nullable: true),
                    ProjectOrganisationOrganisationId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectId);
                    table.ForeignKey(
                        name: "FK_Projects_Organisations_ProjectOrganisationOrganisationId",
                        column: x => x.ProjectOrganisationOrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "OrganisationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "State",
                columns: table => new
                {
                    StateId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StateName = table.Column<string>(nullable: true),
                    StateFullName = table.Column<string>(nullable: true),
                    StateIso = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_State", x => x.StateId);
                });

            migrationBuilder.CreateTable(
                name: "User",
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
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Conferences",
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
                    table.PrimaryKey("PK_Conferences", x => x.ConferenceId);
                    table.ForeignKey(
                        name: "FK_Conferences_Projects_ConferenceProjectProjectId",
                        column: x => x.ConferenceProjectProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conferences_User_CreationUserUserId",
                        column: x => x.CreationUserUserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Committees",
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
                    table.PrimaryKey("PK_Committees", x => x.CommitteeId);
                    table.ForeignKey(
                        name: "FK_Committees_Conferences_ConferenceId",
                        column: x => x.ConferenceId,
                        principalTable: "Conferences",
                        principalColumn: "ConferenceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Committees_Committees_ResolutlyCommitteeCommitteeId",
                        column: x => x.ResolutlyCommitteeCommitteeId,
                        principalTable: "Committees",
                        principalColumn: "CommitteeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NgoRoles",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(nullable: true),
                    ConferenceId = table.Column<string>(nullable: true),
                    IconName = table.Column<string>(nullable: true),
                    ApplicationState = table.Column<int>(nullable: false),
                    ApplicationValue = table.Column<string>(nullable: true),
                    Group = table.Column<int>(nullable: false),
                    Leader = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NgoRoles", x => x.RoleId);
                    table.ForeignKey(
                        name: "FK_NgoRoles_Conferences_ConferenceId",
                        column: x => x.ConferenceId,
                        principalTable: "Conferences",
                        principalColumn: "ConferenceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PressRoles",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(nullable: true),
                    ConferenceId = table.Column<string>(nullable: true),
                    IconName = table.Column<string>(nullable: true),
                    ApplicationState = table.Column<int>(nullable: false),
                    ApplicationValue = table.Column<string>(nullable: true),
                    PressCategory = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PressRoles", x => x.RoleId);
                    table.ForeignKey(
                        name: "FK_PressRoles_Conferences_ConferenceId",
                        column: x => x.ConferenceId,
                        principalTable: "Conferences",
                        principalColumn: "ConferenceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamRoles",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(nullable: true),
                    ConferenceId = table.Column<string>(nullable: true),
                    IconName = table.Column<string>(nullable: true),
                    ApplicationState = table.Column<int>(nullable: false),
                    ApplicationValue = table.Column<string>(nullable: true),
                    ParentTeamRoleRoleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamRoles", x => x.RoleId);
                    table.ForeignKey(
                        name: "FK_TeamRoles_Conferences_ConferenceId",
                        column: x => x.ConferenceId,
                        principalTable: "Conferences",
                        principalColumn: "ConferenceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamRoles_TeamRoles_ParentTeamRoleRoleId",
                        column: x => x.ParentTeamRoleRoleId,
                        principalTable: "TeamRoles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Visitors",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(nullable: true),
                    ConferenceId = table.Column<string>(nullable: true),
                    IconName = table.Column<string>(nullable: true),
                    ApplicationState = table.Column<int>(nullable: false),
                    ApplicationValue = table.Column<string>(nullable: true),
                    Organisation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visitors", x => x.RoleId);
                    table.ForeignKey(
                        name: "FK_Visitors_Conferences_ConferenceId",
                        column: x => x.ConferenceId,
                        principalTable: "Conferences",
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
                        name: "FK_CommitteeTopic_Committees_CommitteeId",
                        column: x => x.CommitteeId,
                        principalTable: "Committees",
                        principalColumn: "CommitteeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Delegates",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(nullable: true),
                    ConferenceId = table.Column<string>(nullable: true),
                    IconName = table.Column<string>(nullable: true),
                    ApplicationState = table.Column<int>(nullable: false),
                    ApplicationValue = table.Column<string>(nullable: true),
                    DelegateStateStateId = table.Column<int>(nullable: true),
                    IsDelegationLeader = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    DelegationId = table.Column<string>(nullable: true),
                    CommitteeId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Delegates", x => x.RoleId);
                    table.ForeignKey(
                        name: "FK_Delegates_Committees_CommitteeId",
                        column: x => x.CommitteeId,
                        principalTable: "Committees",
                        principalColumn: "CommitteeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Delegates_Conferences_ConferenceId",
                        column: x => x.ConferenceId,
                        principalTable: "Conferences",
                        principalColumn: "ConferenceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Delegates_State_DelegateStateStateId",
                        column: x => x.DelegateStateStateId,
                        principalTable: "State",
                        principalColumn: "StateId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Delegates_Delegation_DelegationId",
                        column: x => x.DelegationId,
                        principalTable: "Delegation",
                        principalColumn: "DelegationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Committees_ConferenceId",
                table: "Committees",
                column: "ConferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Committees_ResolutlyCommitteeCommitteeId",
                table: "Committees",
                column: "ResolutlyCommitteeCommitteeId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitteeTopic_CommitteeId",
                table: "CommitteeTopic",
                column: "CommitteeId");

            migrationBuilder.CreateIndex(
                name: "IX_Conferences_ConferenceProjectProjectId",
                table: "Conferences",
                column: "ConferenceProjectProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Conferences_CreationUserUserId",
                table: "Conferences",
                column: "CreationUserUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Delegates_CommitteeId",
                table: "Delegates",
                column: "CommitteeId");

            migrationBuilder.CreateIndex(
                name: "IX_Delegates_ConferenceId",
                table: "Delegates",
                column: "ConferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Delegates_DelegateStateStateId",
                table: "Delegates",
                column: "DelegateStateStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Delegates_DelegationId",
                table: "Delegates",
                column: "DelegationId");

            migrationBuilder.CreateIndex(
                name: "IX_NgoRoles_ConferenceId",
                table: "NgoRoles",
                column: "ConferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_PressRoles_ConferenceId",
                table: "PressRoles",
                column: "ConferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectOrganisationOrganisationId",
                table: "Projects",
                column: "ProjectOrganisationOrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamRoles_ConferenceId",
                table: "TeamRoles",
                column: "ConferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamRoles_ParentTeamRoleRoleId",
                table: "TeamRoles",
                column: "ParentTeamRoleRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_ConferenceId",
                table: "Visitors",
                column: "ConferenceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommitteeTopic");

            migrationBuilder.DropTable(
                name: "Delegates");

            migrationBuilder.DropTable(
                name: "NgoRoles");

            migrationBuilder.DropTable(
                name: "PressRoles");

            migrationBuilder.DropTable(
                name: "TeamRoles");

            migrationBuilder.DropTable(
                name: "Visitors");

            migrationBuilder.DropTable(
                name: "Committees");

            migrationBuilder.DropTable(
                name: "State");

            migrationBuilder.DropTable(
                name: "Delegation");

            migrationBuilder.DropTable(
                name: "Conferences");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
