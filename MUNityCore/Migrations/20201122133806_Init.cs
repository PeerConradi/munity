using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MUNityCore.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Continent = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(150) CHARACTER SET utf8mb4", maxLength: 150, nullable: true),
                    FullName = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: true),
                    Iso = table.Column<string>(type: "varchar(3) CHARACTER SET utf8mb4", maxLength: 3, nullable: true),
                    CountryTimestamp = table.Column<DateTime>(type: "timestamp(6)", rowVersion: true, nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "GroupedRoleApplications",
                columns: table => new
                {
                    GroupedRoleApplicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    GroupedRoleApplicationTimestamp = table.Column<DateTime>(type: "timestamp(6)", rowVersion: true, nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupedRoleApplications", x => x.GroupedRoleApplicationId);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    OrganizationId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    OrganizationName = table.Column<string>(type: "varchar(150) CHARACTER SET utf8mb4", maxLength: 150, nullable: true),
                    OrganizationAbbreviation = table.Column<string>(type: "varchar(18) CHARACTER SET utf8mb4", maxLength: 18, nullable: true),
                    OrganizationTimestamp = table.Column<DateTime>(type: "timestamp(6)", rowVersion: true, nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.OrganizationId);
                });

            migrationBuilder.CreateTable(
                name: "ResolutionAuths",
                columns: table => new
                {
                    ResolutionId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    CreationUserId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastChangeTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AllowPublicRead = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowPublicEdit = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowConferenceRead = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowCommitteeRead = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PublicShortKey = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ConferenceId = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CommitteeId = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResolutionAuths", x => x.ResolutionId);
                });

            migrationBuilder.CreateTable(
                name: "TeamRoleGroups",
                columns: table => new
                {
                    TeamRoleGroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(150) CHARACTER SET utf8mb4", maxLength: 150, nullable: false),
                    FullName = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: true),
                    Abbreviation = table.Column<string>(type: "varchar(10) CHARACTER SET utf8mb4", maxLength: 10, nullable: true),
                    GroupLevel = table.Column<int>(type: "int", nullable: false),
                    TeamRoleGroupTimestamp = table.Column<DateTime>(type: "timestamp(6)", rowVersion: true, nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamRoleGroups", x => x.TeamRoleGroupId);
                });

            migrationBuilder.CreateTable(
                name: "UserAuths",
                columns: table => new
                {
                    MunityUserAuthId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserAuthName = table.Column<string>(type: "varchar(150) CHARACTER SET utf8mb4", maxLength: 150, nullable: true),
                    CanCreateOrganization = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AuthLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAuths", x => x.MunityUserAuthId);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationRoles",
                columns: table => new
                {
                    OrganizationRoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(type: "varchar(150) CHARACTER SET utf8mb4", maxLength: 150, nullable: true),
                    OrganizationId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    CanCreateProject = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationRoles", x => x.OrganizationRoleId);
                    table.ForeignKey(
                        name: "FK_OrganizationRoles_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    ProjectName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ProjectShort = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ProjectOrganizationOrganizationId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    ProjectTimestamp = table.Column<DateTime>(type: "timestamp(6)", rowVersion: true, nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectId);
                    table.ForeignKey(
                        name: "FK_Projects_Organizations_ProjectOrganizationOrganizationId",
                        column: x => x.ProjectOrganizationOrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupApplications",
                columns: table => new
                {
                    GroupApplicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    DelegationId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    Title = table.Column<string>(type: "varchar(150) CHARACTER SET utf8mb4", maxLength: 150, nullable: false),
                    Content = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ApplicationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    GroupApplicationTimestamp = table.Column<DateTime>(type: "timestamp(6)", rowVersion: true, nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupApplications", x => x.GroupApplicationId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    MunityUserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "varchar(40) CHARACTER SET utf8mb4", maxLength: 40, nullable: true),
                    Password = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: true),
                    Mail = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: true),
                    Salt = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: true),
                    Title = table.Column<string>(type: "varchar(100) CHARACTER SET utf8mb4", maxLength: 100, nullable: true),
                    Forename = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: true),
                    Lastname = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: true),
                    Gender = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: true),
                    Birthday = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Country = table.Column<string>(type: "varchar(300) CHARACTER SET utf8mb4", maxLength: 300, nullable: true),
                    Street = table.Column<string>(type: "varchar(300) CHARACTER SET utf8mb4", maxLength: 300, nullable: true),
                    Zipcode = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: true),
                    City = table.Column<string>(type: "varchar(300) CHARACTER SET utf8mb4", maxLength: 300, nullable: true),
                    HouseNumber = table.Column<string>(type: "varchar(20) CHARACTER SET utf8mb4", maxLength: 20, nullable: true),
                    ProfileImageName = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastOnline = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AuthMunityUserAuthId = table.Column<int>(type: "int", nullable: true),
                    UserState = table.Column<int>(type: "int", nullable: false),
                    UserTimestamp = table.Column<DateTime>(type: "timestamp(6)", rowVersion: true, nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    GroupApplicationId = table.Column<int>(type: "int", nullable: true),
                    MunityUserId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.MunityUserId);
                    table.ForeignKey(
                        name: "FK_Users_GroupApplications_GroupApplicationId",
                        column: x => x.GroupApplicationId,
                        principalTable: "GroupApplications",
                        principalColumn: "GroupApplicationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_UserAuths_AuthMunityUserAuthId",
                        column: x => x.AuthMunityUserAuthId,
                        principalTable: "UserAuths",
                        principalColumn: "MunityUserAuthId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Users_MunityUserId1",
                        column: x => x.MunityUserId1,
                        principalTable: "Users",
                        principalColumn: "MunityUserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Conferences",
                columns: table => new
                {
                    ConferenceId = table.Column<string>(type: "varchar(80) CHARACTER SET utf8mb4", maxLength: 80, nullable: false),
                    Name = table.Column<string>(type: "varchar(150) CHARACTER SET utf8mb4", maxLength: 150, nullable: true),
                    FullName = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: true),
                    ConferenceShort = table.Column<string>(type: "varchar(18) CHARACTER SET utf8mb4", maxLength: 18, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreationUserMunityUserId = table.Column<int>(type: "int", nullable: true),
                    ConferenceProjectProjectId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    Visibility = table.Column<int>(type: "int", nullable: false),
                    ConferenceTimestamp = table.Column<DateTime>(type: "timestamp(6)", rowVersion: true, nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
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
                        name: "FK_Conferences_Users_CreationUserMunityUserId",
                        column: x => x.CreationUserMunityUserId,
                        principalTable: "Users",
                        principalColumn: "MunityUserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationMember",
                columns: table => new
                {
                    OrganizationMemberId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserMunityUserId = table.Column<int>(type: "int", nullable: true),
                    OrganizationId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    RoleOrganizationRoleId = table.Column<int>(type: "int", nullable: true),
                    JoinedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    OrganizationMemberTimestamp = table.Column<DateTime>(type: "timestamp(6)", rowVersion: true, nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationMember", x => x.OrganizationMemberId);
                    table.ForeignKey(
                        name: "FK_OrganizationMember_OrganizationRoles_RoleOrganizationRoleId",
                        column: x => x.RoleOrganizationRoleId,
                        principalTable: "OrganizationRoles",
                        principalColumn: "OrganizationRoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrganizationMember_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrganizationMember_Users_UserMunityUserId",
                        column: x => x.UserMunityUserId,
                        principalTable: "Users",
                        principalColumn: "MunityUserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResolutionUsers",
                columns: table => new
                {
                    ResolutionUserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserMunityUserId = table.Column<int>(type: "int", nullable: true),
                    CanRead = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanWrite = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanAddUsers = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AuthResolutionId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_ResolutionUsers_Users_UserMunityUserId",
                        column: x => x.UserMunityUserId,
                        principalTable: "Users",
                        principalColumn: "MunityUserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserPrivacySettings",
                columns: table => new
                {
                    UserPrivacySettingsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserRef = table.Column<int>(type: "int", nullable: false),
                    PublicNameDisplayMode = table.Column<int>(type: "int", nullable: false),
                    InternalNameDisplayMode = table.Column<int>(type: "int", nullable: false),
                    ConferenceNameDisplayMode = table.Column<int>(type: "int", nullable: false),
                    ConferenceHistory = table.Column<int>(type: "int", nullable: false),
                    Friendslist = table.Column<int>(type: "int", nullable: false),
                    Pinboard = table.Column<int>(type: "int", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPrivacySettings", x => x.UserPrivacySettingsId);
                    table.ForeignKey(
                        name: "FK_UserPrivacySettings_Users_UserRef",
                        column: x => x.UserRef,
                        principalTable: "Users",
                        principalColumn: "MunityUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Committees",
                columns: table => new
                {
                    CommitteeId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    Name = table.Column<string>(type: "varchar(150) CHARACTER SET utf8mb4", maxLength: 150, nullable: false),
                    FullName = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: true),
                    CommitteeShort = table.Column<string>(type: "varchar(10) CHARACTER SET utf8mb4", maxLength: 10, nullable: true),
                    Article = table.Column<string>(type: "varchar(10) CHARACTER SET utf8mb4", maxLength: 10, nullable: true),
                    ResolutlyCommitteeCommitteeId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    ConferenceId = table.Column<string>(type: "varchar(80) CHARACTER SET utf8mb4", nullable: true),
                    CommitteeTimestamp = table.Column<DateTime>(type: "timestamp(6)", rowVersion: true, nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Committees", x => x.CommitteeId);
                    table.ForeignKey(
                        name: "FK_Committees_Committees_ResolutlyCommitteeCommitteeId",
                        column: x => x.ResolutlyCommitteeCommitteeId,
                        principalTable: "Committees",
                        principalColumn: "CommitteeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Committees_Conferences_ConferenceId",
                        column: x => x.ConferenceId,
                        principalTable: "Conferences",
                        principalColumn: "ConferenceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Delegation",
                columns: table => new
                {
                    DelegationId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    Name = table.Column<string>(type: "varchar(150) CHARACTER SET utf8mb4", maxLength: 150, nullable: false),
                    FullName = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: true),
                    Abbreviation = table.Column<string>(type: "varchar(10) CHARACTER SET utf8mb4", maxLength: 10, nullable: true),
                    ConferenceId = table.Column<string>(type: "varchar(80) CHARACTER SET utf8mb4", nullable: true)
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
                    RoleAuthId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleAuthName = table.Column<string>(type: "varchar(150) CHARACTER SET utf8mb4", maxLength: 150, nullable: true),
                    PowerLevel = table.Column<int>(type: "int", nullable: false),
                    ConferenceId = table.Column<string>(type: "varchar(80) CHARACTER SET utf8mb4", nullable: true),
                    CanEditConferenceSettings = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanSeeApplications = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanEditParticipations = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RoleAuthTimestamp = table.Column<DateTime>(type: "timestamp(6)", rowVersion: true, nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
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
                name: "CommitteeSession",
                columns: table => new
                {
                    CommitteeSessionId = table.Column<string>(type: "varchar(80) CHARACTER SET utf8mb4", maxLength: 80, nullable: false),
                    CommitteeId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommitteeSession", x => x.CommitteeSessionId);
                    table.ForeignKey(
                        name: "FK_CommitteeSession_Committees_CommitteeId",
                        column: x => x.CommitteeId,
                        principalTable: "Committees",
                        principalColumn: "CommitteeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CommitteeTopic",
                columns: table => new
                {
                    CommitteeTopicId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TopicName = table.Column<string>(type: "varchar(150) CHARACTER SET utf8mb4", maxLength: 150, nullable: true),
                    TopicFullName = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: true),
                    TopicDescription = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    TopicCode = table.Column<string>(type: "varchar(18) CHARACTER SET utf8mb4", maxLength: 18, nullable: true),
                    CommitteeTopicTimestamp = table.Column<DateTime>(type: "timestamp(6)", rowVersion: true, nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    CommitteeId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true)
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
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(type: "varchar(150) CHARACTER SET utf8mb4", maxLength: 150, nullable: true),
                    RoleFullName = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: true),
                    RoleShort = table.Column<string>(type: "varchar(10) CHARACTER SET utf8mb4", maxLength: 10, nullable: true),
                    ConferenceId = table.Column<string>(type: "varchar(80) CHARACTER SET utf8mb4", nullable: true),
                    RoleAuthId = table.Column<int>(type: "int", nullable: true),
                    IconName = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: true),
                    ApplicationState = table.Column<int>(type: "int", nullable: false),
                    ApplicationValue = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: true),
                    AllowMultipleParticipations = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RoleType = table.Column<string>(type: "varchar(150) CHARACTER SET utf8mb4", maxLength: 150, nullable: false),
                    RoleTimestamp = table.Column<DateTime>(type: "timestamp(6)", rowVersion: true, nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    DelegateStateCountryId = table.Column<int>(type: "int", nullable: true),
                    IsDelegationLeader = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Title = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    DelegationId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    CommitteeId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    Group = table.Column<int>(type: "int", nullable: true),
                    NgoName = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: true),
                    Leader = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    PressCategory = table.Column<int>(type: "int", nullable: true),
                    SecretaryGeneralRole_Title = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: true),
                    ParentTeamRoleRoleId = table.Column<int>(type: "int", nullable: true),
                    TeamRoleLevel = table.Column<int>(type: "int", nullable: true),
                    TeamRoleGroupId = table.Column<int>(type: "int", nullable: true),
                    Organization = table.Column<string>(type: "varchar(100) CHARACTER SET utf8mb4", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbstractRole", x => x.RoleId);
                    table.ForeignKey(
                        name: "FK_AbstractRole_AbstractRole_ParentTeamRoleRoleId",
                        column: x => x.ParentTeamRoleRoleId,
                        principalTable: "AbstractRole",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbstractRole_Committees_CommitteeId",
                        column: x => x.CommitteeId,
                        principalTable: "Committees",
                        principalColumn: "CommitteeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbstractRole_Conferences_ConferenceId",
                        column: x => x.ConferenceId,
                        principalTable: "Conferences",
                        principalColumn: "ConferenceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbstractRole_Countries_DelegateStateCountryId",
                        column: x => x.DelegateStateCountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbstractRole_Delegation_DelegationId",
                        column: x => x.DelegationId,
                        principalTable: "Delegation",
                        principalColumn: "DelegationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbstractRole_RoleAuths_RoleAuthId",
                        column: x => x.RoleAuthId,
                        principalTable: "RoleAuths",
                        principalColumn: "RoleAuthId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbstractRole_TeamRoleGroups_TeamRoleGroupId",
                        column: x => x.TeamRoleGroupId,
                        principalTable: "TeamRoleGroups",
                        principalColumn: "TeamRoleGroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Participations",
                columns: table => new
                {
                    ParticipationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    UserMunityUserId = table.Column<int>(type: "int", nullable: true),
                    IsMainRole = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Cost = table.Column<double>(type: "double", nullable: false),
                    Paid = table.Column<double>(type: "double", nullable: false),
                    ParticipationSecret = table.Column<string>(type: "varchar(200) CHARACTER SET utf8mb4", maxLength: 200, nullable: true),
                    ParticipationTimestamp = table.Column<DateTime>(type: "timestamp(6)", rowVersion: true, nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
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
                        name: "FK_Participations_Users_UserMunityUserId",
                        column: x => x.UserMunityUserId,
                        principalTable: "Users",
                        principalColumn: "MunityUserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleApplications",
                columns: table => new
                {
                    RoleApplicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserMunityUserId = table.Column<int>(type: "int", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    ApplyDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Title = table.Column<string>(type: "varchar(200) CHARACTER SET utf8mb4", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    RoleApplicationTimestamp = table.Column<DateTime>(type: "timestamp(6)", rowVersion: true, nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    GroupedRoleApplicationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleApplications", x => x.RoleApplicationId);
                    table.ForeignKey(
                        name: "FK_RoleApplications_AbstractRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AbstractRole",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleApplications_GroupedRoleApplications_GroupedRoleApplicat~",
                        column: x => x.GroupedRoleApplicationId,
                        principalTable: "GroupedRoleApplications",
                        principalColumn: "GroupedRoleApplicationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleApplications_Users_UserMunityUserId",
                        column: x => x.UserMunityUserId,
                        principalTable: "Users",
                        principalColumn: "MunityUserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Simulations",
                columns: table => new
                {
                    SimulationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    LobbyMode = table.Column<int>(type: "int", nullable: false),
                    Password = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CanJoin = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ListOfSpeakersId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Simulations", x => x.SimulationId);
                });

            migrationBuilder.CreateTable(
                name: "AllChatMessage",
                columns: table => new
                {
                    AllChatMessageId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    AuthorName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Text = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SimulationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllChatMessage", x => x.AllChatMessageId);
                    table.ForeignKey(
                        name: "FK_AllChatMessage_Simulations_SimulationId",
                        column: x => x.SimulationId,
                        principalTable: "Simulations",
                        principalColumn: "SimulationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SimSimRequestModel",
                columns: table => new
                {
                    SimSimRequestModelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserToken = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    RequestType = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Message = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    RequestTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SimulationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimSimRequestModel", x => x.SimSimRequestModelId);
                    table.ForeignKey(
                        name: "FK_SimSimRequestModel_Simulations_SimulationId",
                        column: x => x.SimulationId,
                        principalTable: "Simulations",
                        principalColumn: "SimulationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SimulationRoles",
                columns: table => new
                {
                    SimulationRoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: true),
                    RoleType = table.Column<int>(type: "int", nullable: false),
                    Iso = table.Column<string>(type: "varchar(2) CHARACTER SET utf8mb4", maxLength: 2, nullable: true),
                    RoleKey = table.Column<string>(type: "varchar(32) CHARACTER SET utf8mb4", maxLength: 32, nullable: true),
                    RoleMaxSlots = table.Column<int>(type: "int", nullable: false),
                    SimulationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimulationRoles", x => x.SimulationRoleId);
                    table.ForeignKey(
                        name: "FK_SimulationRoles_Simulations_SimulationId",
                        column: x => x.SimulationId,
                        principalTable: "Simulations",
                        principalColumn: "SimulationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SimulationUser",
                columns: table => new
                {
                    SimulationUserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DisplayName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    RoleSimulationRoleId = table.Column<int>(type: "int", nullable: true),
                    CanCreateRole = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanSelectRole = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanEditResolution = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanEditListOfSpeakers = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SimulationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimulationUser", x => x.SimulationUserId);
                    table.ForeignKey(
                        name: "FK_SimulationUser_SimulationRoles_RoleSimulationRoleId",
                        column: x => x.RoleSimulationRoleId,
                        principalTable: "SimulationRoles",
                        principalColumn: "SimulationRoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SimulationUser_Simulations_SimulationId",
                        column: x => x.SimulationId,
                        principalTable: "Simulations",
                        principalColumn: "SimulationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Speakers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Iso = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ListOfSpeakersId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    ListOfSpeakersId1 = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Speakers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ListOfSpeakers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    PublicId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    SpeakerTime = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    QuestionTime = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    RemainingSpeakerTime = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    RemainingQuestionTime = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    CurrentSpeakerId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    CurrentQuestionId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ListClosed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    QuestionsClosed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LowTimeMark = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    SpeakerLowTime = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    QuestionLowTime = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SpeakerTimeout = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    QuestionTimeout = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ConferenceId = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CommitteeId = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    StartSpeakerTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    StartQuestionTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListOfSpeakers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListOfSpeakers_Speakers_CurrentQuestionId",
                        column: x => x.CurrentQuestionId,
                        principalTable: "Speakers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ListOfSpeakers_Speakers_CurrentSpeakerId",
                        column: x => x.CurrentSpeakerId,
                        principalTable: "Speakers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbstractRole_CommitteeId",
                table: "AbstractRole",
                column: "CommitteeId");

            migrationBuilder.CreateIndex(
                name: "IX_AbstractRole_ConferenceId",
                table: "AbstractRole",
                column: "ConferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_AbstractRole_DelegateStateCountryId",
                table: "AbstractRole",
                column: "DelegateStateCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_AbstractRole_DelegationId",
                table: "AbstractRole",
                column: "DelegationId");

            migrationBuilder.CreateIndex(
                name: "IX_AbstractRole_ParentTeamRoleRoleId",
                table: "AbstractRole",
                column: "ParentTeamRoleRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AbstractRole_RoleAuthId",
                table: "AbstractRole",
                column: "RoleAuthId");

            migrationBuilder.CreateIndex(
                name: "IX_AbstractRole_TeamRoleGroupId",
                table: "AbstractRole",
                column: "TeamRoleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AllChatMessage_SimulationId",
                table: "AllChatMessage",
                column: "SimulationId");

            migrationBuilder.CreateIndex(
                name: "IX_Committees_ConferenceId",
                table: "Committees",
                column: "ConferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Committees_ResolutlyCommitteeCommitteeId",
                table: "Committees",
                column: "ResolutlyCommitteeCommitteeId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitteeSession_CommitteeId",
                table: "CommitteeSession",
                column: "CommitteeId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitteeTopic_CommitteeId",
                table: "CommitteeTopic",
                column: "CommitteeId");

            migrationBuilder.CreateIndex(
                name: "IX_Conferences_ConferenceProjectProjectId",
                table: "Conferences",
                column: "ConferenceProjectProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Conferences_CreationUserMunityUserId",
                table: "Conferences",
                column: "CreationUserMunityUserId");

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
                name: "IX_ListOfSpeakers_CurrentQuestionId",
                table: "ListOfSpeakers",
                column: "CurrentQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ListOfSpeakers_CurrentSpeakerId",
                table: "ListOfSpeakers",
                column: "CurrentSpeakerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationMember_OrganizationId",
                table: "OrganizationMember",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationMember_RoleOrganizationRoleId",
                table: "OrganizationMember",
                column: "RoleOrganizationRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationMember_UserMunityUserId",
                table: "OrganizationMember",
                column: "UserMunityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationRoles_OrganizationId",
                table: "OrganizationRoles",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Participations_RoleId",
                table: "Participations",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Participations_UserMunityUserId",
                table: "Participations",
                column: "UserMunityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectOrganizationOrganizationId",
                table: "Projects",
                column: "ProjectOrganizationOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionUsers_AuthResolutionId",
                table: "ResolutionUsers",
                column: "AuthResolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionUsers_UserMunityUserId",
                table: "ResolutionUsers",
                column: "UserMunityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleApplications_GroupedRoleApplicationId",
                table: "RoleApplications",
                column: "GroupedRoleApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleApplications_RoleId",
                table: "RoleApplications",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleApplications_UserMunityUserId",
                table: "RoleApplications",
                column: "UserMunityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleAuths_ConferenceId",
                table: "RoleAuths",
                column: "ConferenceId");

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
                name: "IX_Speakers_ListOfSpeakersId",
                table: "Speakers",
                column: "ListOfSpeakersId");

            migrationBuilder.CreateIndex(
                name: "IX_Speakers_ListOfSpeakersId1",
                table: "Speakers",
                column: "ListOfSpeakersId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserPrivacySettings_UserRef",
                table: "UserPrivacySettings",
                column: "UserRef",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_AuthMunityUserAuthId",
                table: "Users",
                column: "AuthMunityUserAuthId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_GroupApplicationId",
                table: "Users",
                column: "GroupApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_MunityUserId1",
                table: "Users",
                column: "MunityUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupApplications_AbstractRole_RoleId",
                table: "GroupApplications",
                column: "RoleId",
                principalTable: "AbstractRole",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupApplications_Delegation_DelegationId",
                table: "GroupApplications",
                column: "DelegationId",
                principalTable: "Delegation",
                principalColumn: "DelegationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Simulations_ListOfSpeakers_ListOfSpeakersId",
                table: "Simulations",
                column: "ListOfSpeakersId",
                principalTable: "ListOfSpeakers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Speakers_ListOfSpeakers_ListOfSpeakersId",
                table: "Speakers",
                column: "ListOfSpeakersId",
                principalTable: "ListOfSpeakers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Speakers_ListOfSpeakers_ListOfSpeakersId1",
                table: "Speakers",
                column: "ListOfSpeakersId1",
                principalTable: "ListOfSpeakers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbstractRole_Committees_CommitteeId",
                table: "AbstractRole");

            migrationBuilder.DropForeignKey(
                name: "FK_AbstractRole_Conferences_ConferenceId",
                table: "AbstractRole");

            migrationBuilder.DropForeignKey(
                name: "FK_Delegation_Conferences_ConferenceId",
                table: "Delegation");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleAuths_Conferences_ConferenceId",
                table: "RoleAuths");

            migrationBuilder.DropForeignKey(
                name: "FK_ListOfSpeakers_Speakers_CurrentQuestionId",
                table: "ListOfSpeakers");

            migrationBuilder.DropForeignKey(
                name: "FK_ListOfSpeakers_Speakers_CurrentSpeakerId",
                table: "ListOfSpeakers");

            migrationBuilder.DropTable(
                name: "AllChatMessage");

            migrationBuilder.DropTable(
                name: "CommitteeSession");

            migrationBuilder.DropTable(
                name: "CommitteeTopic");

            migrationBuilder.DropTable(
                name: "OrganizationMember");

            migrationBuilder.DropTable(
                name: "Participations");

            migrationBuilder.DropTable(
                name: "ResolutionUsers");

            migrationBuilder.DropTable(
                name: "RoleApplications");

            migrationBuilder.DropTable(
                name: "SimSimRequestModel");

            migrationBuilder.DropTable(
                name: "SimulationUser");

            migrationBuilder.DropTable(
                name: "UserPrivacySettings");

            migrationBuilder.DropTable(
                name: "OrganizationRoles");

            migrationBuilder.DropTable(
                name: "ResolutionAuths");

            migrationBuilder.DropTable(
                name: "GroupedRoleApplications");

            migrationBuilder.DropTable(
                name: "SimulationRoles");

            migrationBuilder.DropTable(
                name: "Simulations");

            migrationBuilder.DropTable(
                name: "Committees");

            migrationBuilder.DropTable(
                name: "Conferences");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "GroupApplications");

            migrationBuilder.DropTable(
                name: "UserAuths");

            migrationBuilder.DropTable(
                name: "AbstractRole");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Delegation");

            migrationBuilder.DropTable(
                name: "RoleAuths");

            migrationBuilder.DropTable(
                name: "TeamRoleGroups");

            migrationBuilder.DropTable(
                name: "Speakers");

            migrationBuilder.DropTable(
                name: "ListOfSpeakers");
        }
    }
}
