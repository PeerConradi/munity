using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MUNityAngular.Migrations.MunCore
{
    public partial class NewCore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupedRoleApplications",
                columns: table => new
                {
                    GroupedRoleApplicationId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupedRoleApplications", x => x.GroupedRoleApplicationId);
                });

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
                    table.PrimaryKey("PK_Organisations", x => x.OrganisationId);
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
                name: "OrganisationRoles",
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
                    table.PrimaryKey("PK_OrganisationRoles", x => x.OrganisationRoleId);
                    table.ForeignKey(
                        name: "FK_OrganisationRoles_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "OrganisationId",
                        onDelete: ReferentialAction.Restrict);
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
                name: "GroupApplications",
                columns: table => new
                {
                    GroupApplicationId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<int>(nullable: true),
                    DelegationId = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    ApplicationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupApplications", x => x.GroupApplicationId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
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
                    LastOnline = table.Column<DateTime>(nullable: false),
                    GroupApplicationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_GroupApplications_GroupApplicationId",
                        column: x => x.GroupApplicationId,
                        principalTable: "GroupApplications",
                        principalColumn: "GroupApplicationId",
                        onDelete: ReferentialAction.Restrict);
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
                        name: "FK_Conferences_Users_CreationUserUserId",
                        column: x => x.CreationUserUserId,
                        principalTable: "Users",
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
                        name: "FK_OrganisationMember_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "OrganisationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrganisationMember_OrganisationRoles_RoleOrganisationRoleId",
                        column: x => x.RoleOrganisationRoleId,
                        principalTable: "OrganisationRoles",
                        principalColumn: "OrganisationRoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrganisationMember_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserAuths",
                columns: table => new
                {
                    UserAuthId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: true),
                    CanCreateOrganisation = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAuths", x => x.UserAuthId);
                    table.ForeignKey(
                        name: "FK_UserAuths_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Committees_Committees_ResolutlyCommitteeCommitteeId",
                        column: x => x.ResolutlyCommitteeCommitteeId,
                        principalTable: "Committees",
                        principalColumn: "CommitteeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Delegation",
                columns: table => new
                {
                    DelegationId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ConferenceId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Delegation", x => x.DelegationId);
                    table.ForeignKey(
                        name: "FK_Delegation_Conferences_ConferenceId",
                        column: x => x.ConferenceId,
                        principalTable: "Conferences",
                        principalColumn: "ConferenceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleAuths",
                columns: table => new
                {
                    RoleAuthId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleAuthName = table.Column<string>(nullable: true),
                    PowerLevel = table.Column<int>(nullable: false),
                    ConferenceId = table.Column<string>(nullable: true),
                    CanEditConferenceSettings = table.Column<bool>(nullable: false),
                    CanSeeApplications = table.Column<bool>(nullable: false),
                    CanEditParticipations = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleAuths", x => x.RoleAuthId);
                    table.ForeignKey(
                        name: "FK_RoleAuths_Conferences_ConferenceId",
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
                name: "AbstractRole",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(nullable: true),
                    ConferenceId = table.Column<string>(nullable: true),
                    RoleAuthId = table.Column<int>(nullable: true),
                    IconName = table.Column<string>(nullable: true),
                    ApplicationState = table.Column<int>(nullable: false),
                    ApplicationValue = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    DelegateStateStateId = table.Column<int>(nullable: true),
                    IsDelegationLeader = table.Column<bool>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    DelegationId = table.Column<string>(nullable: true),
                    CommitteeId = table.Column<string>(nullable: true),
                    Group = table.Column<int>(nullable: true),
                    NgoName = table.Column<string>(nullable: true),
                    Leader = table.Column<bool>(nullable: true),
                    PressCategory = table.Column<int>(nullable: true),
                    SecretaryGeneralRole_Title = table.Column<string>(nullable: true),
                    ParentTeamRoleRoleId = table.Column<int>(nullable: true),
                    TeamRoleGroup = table.Column<int>(nullable: true),
                    Organisation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbstractRole", x => x.RoleId);
                    table.ForeignKey(
                        name: "FK_AbstractRole_Conferences_ConferenceId",
                        column: x => x.ConferenceId,
                        principalTable: "Conferences",
                        principalColumn: "ConferenceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbstractRole_RoleAuths_RoleAuthId",
                        column: x => x.RoleAuthId,
                        principalTable: "RoleAuths",
                        principalColumn: "RoleAuthId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbstractRole_Committees_CommitteeId",
                        column: x => x.CommitteeId,
                        principalTable: "Committees",
                        principalColumn: "CommitteeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbstractRole_State_DelegateStateStateId",
                        column: x => x.DelegateStateStateId,
                        principalTable: "State",
                        principalColumn: "StateId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbstractRole_Delegation_DelegationId",
                        column: x => x.DelegationId,
                        principalTable: "Delegation",
                        principalColumn: "DelegationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbstractRole_AbstractRole_ParentTeamRoleRoleId",
                        column: x => x.ParentTeamRoleRoleId,
                        principalTable: "AbstractRole",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Participations",
                columns: table => new
                {
                    ParticipationId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<int>(nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    IsMainRole = table.Column<bool>(nullable: false),
                    Cost = table.Column<double>(nullable: false),
                    Paid = table.Column<double>(nullable: false),
                    ParticipationSecret = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participations", x => x.ParticipationId);
                    table.ForeignKey(
                        name: "FK_Participations_AbstractRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AbstractRole",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Participations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleApplications",
                columns: table => new
                {
                    RoleApplicationId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: true),
                    RoleId = table.Column<int>(nullable: true),
                    ApplyDate = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    GroupedRoleApplicationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleApplications", x => x.RoleApplicationId);
                    table.ForeignKey(
                        name: "FK_RoleApplications_GroupedRoleApplications_GroupedRoleApplicat~",
                        column: x => x.GroupedRoleApplicationId,
                        principalTable: "GroupedRoleApplications",
                        principalColumn: "GroupedRoleApplicationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleApplications_AbstractRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AbstractRole",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleApplications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbstractRole_ConferenceId",
                table: "AbstractRole",
                column: "ConferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_AbstractRole_RoleAuthId",
                table: "AbstractRole",
                column: "RoleAuthId");

            migrationBuilder.CreateIndex(
                name: "IX_AbstractRole_CommitteeId",
                table: "AbstractRole",
                column: "CommitteeId");

            migrationBuilder.CreateIndex(
                name: "IX_AbstractRole_DelegateStateStateId",
                table: "AbstractRole",
                column: "DelegateStateStateId");

            migrationBuilder.CreateIndex(
                name: "IX_AbstractRole_DelegationId",
                table: "AbstractRole",
                column: "DelegationId");

            migrationBuilder.CreateIndex(
                name: "IX_AbstractRole_ParentTeamRoleRoleId",
                table: "AbstractRole",
                column: "ParentTeamRoleRoleId");

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
                name: "IX_Delegation_ConferenceId",
                table: "Delegation",
                column: "ConferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupApplications_DelegationId",
                table: "GroupApplications",
                column: "DelegationId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupApplications_RoleId",
                table: "GroupApplications",
                column: "RoleId");

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
                name: "IX_OrganisationRoles_OrganisationId",
                table: "OrganisationRoles",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_Participations_RoleId",
                table: "Participations",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Participations_UserId",
                table: "Participations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectOrganisationOrganisationId",
                table: "Projects",
                column: "ProjectOrganisationOrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleApplications_GroupedRoleApplicationId",
                table: "RoleApplications",
                column: "GroupedRoleApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleApplications_RoleId",
                table: "RoleApplications",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleApplications_UserId",
                table: "RoleApplications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleAuths_ConferenceId",
                table: "RoleAuths",
                column: "ConferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAuths_UserId",
                table: "UserAuths",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_GroupApplicationId",
                table: "Users",
                column: "GroupApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupApplications_Delegation_DelegationId",
                table: "GroupApplications",
                column: "DelegationId",
                principalTable: "Delegation",
                principalColumn: "DelegationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupApplications_AbstractRole_RoleId",
                table: "GroupApplications",
                column: "RoleId",
                principalTable: "AbstractRole",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbstractRole_Conferences_ConferenceId",
                table: "AbstractRole");

            migrationBuilder.DropForeignKey(
                name: "FK_Committees_Conferences_ConferenceId",
                table: "Committees");

            migrationBuilder.DropForeignKey(
                name: "FK_Delegation_Conferences_ConferenceId",
                table: "Delegation");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleAuths_Conferences_ConferenceId",
                table: "RoleAuths");

            migrationBuilder.DropTable(
                name: "CommitteeTopic");

            migrationBuilder.DropTable(
                name: "OrganisationMember");

            migrationBuilder.DropTable(
                name: "Participations");

            migrationBuilder.DropTable(
                name: "RoleApplications");

            migrationBuilder.DropTable(
                name: "UserAuths");

            migrationBuilder.DropTable(
                name: "OrganisationRoles");

            migrationBuilder.DropTable(
                name: "GroupedRoleApplications");

            migrationBuilder.DropTable(
                name: "Conferences");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Organisations");

            migrationBuilder.DropTable(
                name: "GroupApplications");

            migrationBuilder.DropTable(
                name: "AbstractRole");

            migrationBuilder.DropTable(
                name: "RoleAuths");

            migrationBuilder.DropTable(
                name: "Committees");

            migrationBuilder.DropTable(
                name: "State");

            migrationBuilder.DropTable(
                name: "Delegation");
        }
    }
}
