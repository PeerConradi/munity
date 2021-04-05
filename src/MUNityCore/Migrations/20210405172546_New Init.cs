using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MUNityCore.Migrations
{
    public partial class NewInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    Name = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

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
                name: "ListOfSpeakers",
                columns: table => new
                {
                    ListOfSpeakersId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    PublicId = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SpeakerTime = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    QuestionTime = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    PausedSpeakerTime = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    PausedQuestionTime = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    ListClosed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    QuestionsClosed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    StartSpeakerTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    StartQuestionTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListOfSpeakers", x => x.ListOfSpeakersId);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    OrganizationId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    OrganizationName = table.Column<string>(type: "varchar(150) CHARACTER SET utf8mb4", maxLength: 150, nullable: true),
                    OrganizationShort = table.Column<string>(type: "varchar(18) CHARACTER SET utf8mb4", maxLength: 18, nullable: true),
                    OrganizationTimestamp = table.Column<DateTime>(type: "timestamp(6)", rowVersion: true, nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.OrganizationId);
                });

            migrationBuilder.CreateTable(
                name: "PetitionTypes",
                columns: table => new
                {
                    PetitionTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Description = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Reference = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Ruling = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetitionTypes", x => x.PetitionTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Resolutions",
                columns: table => new
                {
                    ResaElementId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    Topic = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    FullName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    AgendaItem = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Session = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    SubmitterName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CommitteeName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SupporterNames = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resolutions", x => x.ResaElementId);
                });

            migrationBuilder.CreateTable(
                name: "TeamRoleGroups",
                columns: table => new
                {
                    TeamRoleGroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(150) CHARACTER SET utf8mb4", maxLength: 150, nullable: false),
                    FullName = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: true),
                    TeamRoleGroupShort = table.Column<string>(type: "varchar(10) CHARACTER SET utf8mb4", maxLength: 10, nullable: true),
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
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Simulations",
                columns: table => new
                {
                    SimulationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Phase = table.Column<int>(type: "int", nullable: false),
                    LobbyMode = table.Column<int>(type: "int", nullable: false),
                    LastStatusChange = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Password = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ListOfSpeakersId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    CurrentResolutionId = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Simulations", x => x.SimulationId);
                    table.ForeignKey(
                        name: "FK_Simulations_ListOfSpeakers_ListOfSpeakersId",
                        column: x => x.ListOfSpeakersId,
                        principalTable: "ListOfSpeakers",
                        principalColumn: "ListOfSpeakersId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Speakers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Iso = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Mode = table.Column<int>(type: "int", nullable: false),
                    OrdnerIndex = table.Column<int>(type: "int", nullable: false),
                    ListOfSpeakersId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Speakers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Speakers_ListOfSpeakers_ListOfSpeakersId",
                        column: x => x.ListOfSpeakersId,
                        principalTable: "ListOfSpeakers",
                        principalColumn: "ListOfSpeakersId",
                        onDelete: ReferentialAction.Restrict);
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
                name: "OperativeParagraphs",
                columns: table => new
                {
                    ResaOperativeParagraphId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Text = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    IsLocked = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsVirtual = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Visible = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Corrected = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Comment = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    ResolutionResaElementId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    ParentResaOperativeParagraphId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperativeParagraphs", x => x.ResaOperativeParagraphId);
                    table.ForeignKey(
                        name: "FK_OperativeParagraphs_OperativeParagraphs_ParentResaOperativeP~",
                        column: x => x.ParentResaOperativeParagraphId,
                        principalTable: "OperativeParagraphs",
                        principalColumn: "ResaOperativeParagraphId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OperativeParagraphs_Resolutions_ResolutionResaElementId",
                        column: x => x.ResolutionResaElementId,
                        principalTable: "Resolutions",
                        principalColumn: "ResaElementId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PreambleParagraphs",
                columns: table => new
                {
                    ResaPreambleParagraphId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    Text = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    IsLocked = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsCorrected = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Comment = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ResaElementId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreambleParagraphs", x => x.ResaPreambleParagraphId);
                    table.ForeignKey(
                        name: "FK_PreambleParagraphs_Resolutions_ResaElementId",
                        column: x => x.ResaElementId,
                        principalTable: "Resolutions",
                        principalColumn: "ResaElementId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResolutionSupporters",
                columns: table => new
                {
                    ResaSupporterId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ResolutionResaElementId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResolutionSupporters", x => x.ResaSupporterId);
                    table.ForeignKey(
                        name: "FK_ResolutionSupporters_Resolutions_ResolutionResaElementId",
                        column: x => x.ResolutionResaElementId,
                        principalTable: "Resolutions",
                        principalColumn: "ResaElementId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AgendaItems",
                columns: table => new
                {
                    AgendaItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Description = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PlannedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DoneDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    SimulationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgendaItems", x => x.AgendaItemId);
                    table.ForeignKey(
                        name: "FK_AgendaItems_Simulations_SimulationId",
                        column: x => x.SimulationId,
                        principalTable: "Simulations",
                        principalColumn: "SimulationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SimulationPetitionTypes",
                columns: table => new
                {
                    PetitionTypeSimulationId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SimulationId = table.Column<int>(type: "int", nullable: true),
                    PetitionTypeId = table.Column<int>(type: "int", nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    AllowChairs = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowDelegates = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowNgo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowSpectator = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimulationPetitionTypes", x => x.PetitionTypeSimulationId);
                    table.ForeignKey(
                        name: "FK_SimulationPetitionTypes_PetitionTypes_PetitionTypeId",
                        column: x => x.PetitionTypeId,
                        principalTable: "PetitionTypes",
                        principalColumn: "PetitionTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SimulationPetitionTypes_Simulations_SimulationId",
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
                    Iso = table.Column<string>(type: "varchar(5) CHARACTER SET utf8mb4", maxLength: 5, nullable: true),
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
                name: "SimulationStatuses",
                columns: table => new
                {
                    SimulationStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StatusText = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    StatusTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SimulationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimulationStatuses", x => x.SimulationStatusId);
                    table.ForeignKey(
                        name: "FK_SimulationStatuses_Simulations_SimulationId",
                        column: x => x.SimulationId,
                        principalTable: "Simulations",
                        principalColumn: "SimulationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SimulationVotings",
                columns: table => new
                {
                    SimulationVotingId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Description = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    SimulationId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowAbstention = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimulationVotings", x => x.SimulationVotingId);
                    table.ForeignKey(
                        name: "FK_SimulationVotings_Simulations_SimulationId",
                        column: x => x.SimulationId,
                        principalTable: "Simulations",
                        principalColumn: "SimulationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Amendments",
                columns: table => new
                {
                    ResaAmendmentId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    ResolutionResaElementId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    SubmitterName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Activated = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SubmitTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Discriminator = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    VirtualParagraphResaOperativeParagraphId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    TargetParagraphResaOperativeParagraphId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    NewText = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ResaDeleteAmendment_TargetParagraphResaOperativeParagraphId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    SourceParagraphResaOperativeParagraphId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    ResaMoveAmendment_VirtualParagraphResaOperativeParagraphId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amendments", x => x.ResaAmendmentId);
                    table.ForeignKey(
                        name: "FK_Amendments_OperativeParagraphs_ResaDeleteAmendment_TargetPar~",
                        column: x => x.ResaDeleteAmendment_TargetParagraphResaOperativeParagraphId,
                        principalTable: "OperativeParagraphs",
                        principalColumn: "ResaOperativeParagraphId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Amendments_OperativeParagraphs_ResaMoveAmendment_VirtualPara~",
                        column: x => x.ResaMoveAmendment_VirtualParagraphResaOperativeParagraphId,
                        principalTable: "OperativeParagraphs",
                        principalColumn: "ResaOperativeParagraphId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Amendments_OperativeParagraphs_SourceParagraphResaOperativeP~",
                        column: x => x.SourceParagraphResaOperativeParagraphId,
                        principalTable: "OperativeParagraphs",
                        principalColumn: "ResaOperativeParagraphId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Amendments_OperativeParagraphs_TargetParagraphResaOperativeP~",
                        column: x => x.TargetParagraphResaOperativeParagraphId,
                        principalTable: "OperativeParagraphs",
                        principalColumn: "ResaOperativeParagraphId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Amendments_OperativeParagraphs_VirtualParagraphResaOperative~",
                        column: x => x.VirtualParagraphResaOperativeParagraphId,
                        principalTable: "OperativeParagraphs",
                        principalColumn: "ResaOperativeParagraphId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Amendments_Resolutions_ResolutionResaElementId",
                        column: x => x.ResolutionResaElementId,
                        principalTable: "Resolutions",
                        principalColumn: "ResaElementId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SimulationUser",
                columns: table => new
                {
                    SimulationUserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Token = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    DisplayName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    PublicUserId = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Password = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    PinRetries = table.Column<int>(type: "int", nullable: false),
                    RoleSimulationRoleId = table.Column<int>(type: "int", nullable: true),
                    CanCreateRole = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanSelectRole = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanEditResolution = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanEditListOfSpeakers = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastKnownConnectionId = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
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
                name: "Petitions",
                columns: table => new
                {
                    PetitionId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    PetitionDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PetitionTypeId = table.Column<int>(type: "int", nullable: true),
                    SimulationUserId = table.Column<int>(type: "int", nullable: true),
                    AgendaItemId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Petitions", x => x.PetitionId);
                    table.ForeignKey(
                        name: "FK_Petitions_AgendaItems_AgendaItemId",
                        column: x => x.AgendaItemId,
                        principalTable: "AgendaItems",
                        principalColumn: "AgendaItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Petitions_PetitionTypes_PetitionTypeId",
                        column: x => x.PetitionTypeId,
                        principalTable: "PetitionTypes",
                        principalColumn: "PetitionTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Petitions_SimulationUser_SimulationUserId",
                        column: x => x.SimulationUserId,
                        principalTable: "SimulationUser",
                        principalColumn: "SimulationUserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SimulationHubConnections",
                columns: table => new
                {
                    SimulationHubConnectionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserSimulationUserId = table.Column<int>(type: "int", nullable: true),
                    ConnectionId = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimulationHubConnections", x => x.SimulationHubConnectionId);
                    table.ForeignKey(
                        name: "FK_SimulationHubConnections_SimulationUser_UserSimulationUserId",
                        column: x => x.UserSimulationUserId,
                        principalTable: "SimulationUser",
                        principalColumn: "SimulationUserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VotingSlots",
                columns: table => new
                {
                    SimulationVotingSlotId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VotingSimulationVotingId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    UserSimulationUserId = table.Column<int>(type: "int", nullable: true),
                    Choice = table.Column<int>(type: "int", nullable: false),
                    VoteTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VotingSlots", x => x.SimulationVotingSlotId);
                    table.ForeignKey(
                        name: "FK_VotingSlots_SimulationUser_UserSimulationUserId",
                        column: x => x.UserSimulationUserId,
                        principalTable: "SimulationUser",
                        principalColumn: "SimulationUserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VotingSlots_SimulationVotings_VotingSimulationVotingId",
                        column: x => x.VotingSimulationVotingId,
                        principalTable: "SimulationVotings",
                        principalColumn: "SimulationVotingId",
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
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
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
                    MunityUserId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    UserName = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    SecurityStamp = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    PhoneNumber = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_AspNetUsers_MunityUserId",
                        column: x => x.MunityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_GroupApplications_GroupApplicationId",
                        column: x => x.GroupApplicationId,
                        principalTable: "GroupApplications",
                        principalColumn: "GroupApplicationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_UserAuths_AuthMunityUserAuthId",
                        column: x => x.AuthMunityUserAuthId,
                        principalTable: "UserAuths",
                        principalColumn: "MunityUserAuthId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    ProviderKey = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    UserId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    RoleId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    LoginProvider = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    Name = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    Value = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    CreationUserId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    ConferenceProjectProjectId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    Visibility = table.Column<int>(type: "int", nullable: false),
                    ConferenceTimestamp = table.Column<DateTime>(type: "timestamp(6)", rowVersion: true, nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conferences", x => x.ConferenceId);
                    table.ForeignKey(
                        name: "FK_Conferences_AspNetUsers_CreationUserId",
                        column: x => x.CreationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conferences_Projects_ConferenceProjectProjectId",
                        column: x => x.ConferenceProjectProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationMember",
                columns: table => new
                {
                    OrganizationMemberId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
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
                        name: "FK_OrganizationMember_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    SetttingName = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    SettingValue = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    SetById = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    ChangeDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.SetttingName);
                    table.ForeignKey(
                        name: "FK_Settings_AspNetUsers_SetById",
                        column: x => x.SetById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserPrivacySettings",
                columns: table => new
                {
                    UserPrivacySettingsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserRef = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
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
                        name: "FK_UserPrivacySettings_AspNetUsers_UserRef",
                        column: x => x.UserRef,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "ResolutionAuths",
                columns: table => new
                {
                    ResolutionId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: false),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreationUserId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastChangeTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AllowPublicRead = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowPublicEdit = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowConferenceRead = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowCommitteeRead = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowOnlineAmendments = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PublicShortKey = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    EditPassword = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ReadPassword = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CommitteeId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    SimulationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResolutionAuths", x => x.ResolutionId);
                    table.ForeignKey(
                        name: "FK_ResolutionAuths_AspNetUsers_CreationUserId",
                        column: x => x.CreationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResolutionAuths_Committees_CommitteeId",
                        column: x => x.CommitteeId,
                        principalTable: "Committees",
                        principalColumn: "CommitteeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResolutionAuths_Simulations_SimulationId",
                        column: x => x.SimulationId,
                        principalTable: "Simulations",
                        principalColumn: "SimulationId",
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
                name: "ResolutionUsers",
                columns: table => new
                {
                    ResolutionUserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    CanRead = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanWrite = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanAddUsers = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AuthResolutionId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResolutionUsers", x => x.ResolutionUserId);
                    table.ForeignKey(
                        name: "FK_ResolutionUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResolutionUsers_ResolutionAuths_AuthResolutionId",
                        column: x => x.AuthResolutionId,
                        principalTable: "ResolutionAuths",
                        principalColumn: "ResolutionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Participations",
                columns: table => new
                {
                    ParticipationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
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
                        name: "FK_Participations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleApplications",
                columns: table => new
                {
                    RoleApplicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
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
                        name: "FK_RoleApplications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleApplications_GroupedRoleApplications_GroupedRoleApplicat~",
                        column: x => x.GroupedRoleApplicationId,
                        principalTable: "GroupedRoleApplications",
                        principalColumn: "GroupedRoleApplicationId",
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
                name: "IX_AgendaItems_SimulationId",
                table: "AgendaItems",
                column: "SimulationId");

            migrationBuilder.CreateIndex(
                name: "IX_Amendments_ResaDeleteAmendment_TargetParagraphResaOperativeP~",
                table: "Amendments",
                column: "ResaDeleteAmendment_TargetParagraphResaOperativeParagraphId");

            migrationBuilder.CreateIndex(
                name: "IX_Amendments_ResaMoveAmendment_VirtualParagraphResaOperativePa~",
                table: "Amendments",
                column: "ResaMoveAmendment_VirtualParagraphResaOperativeParagraphId");

            migrationBuilder.CreateIndex(
                name: "IX_Amendments_ResolutionResaElementId",
                table: "Amendments",
                column: "ResolutionResaElementId");

            migrationBuilder.CreateIndex(
                name: "IX_Amendments_SourceParagraphResaOperativeParagraphId",
                table: "Amendments",
                column: "SourceParagraphResaOperativeParagraphId");

            migrationBuilder.CreateIndex(
                name: "IX_Amendments_TargetParagraphResaOperativeParagraphId",
                table: "Amendments",
                column: "TargetParagraphResaOperativeParagraphId");

            migrationBuilder.CreateIndex(
                name: "IX_Amendments_VirtualParagraphResaOperativeParagraphId",
                table: "Amendments",
                column: "VirtualParagraphResaOperativeParagraphId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AuthMunityUserAuthId",
                table: "AspNetUsers",
                column: "AuthMunityUserAuthId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GroupApplicationId",
                table: "AspNetUsers",
                column: "GroupApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_MunityUserId",
                table: "AspNetUsers",
                column: "MunityUserId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

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
                name: "IX_Conferences_CreationUserId",
                table: "Conferences",
                column: "CreationUserId");

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
                name: "IX_OperativeParagraphs_ParentResaOperativeParagraphId",
                table: "OperativeParagraphs",
                column: "ParentResaOperativeParagraphId");

            migrationBuilder.CreateIndex(
                name: "IX_OperativeParagraphs_ResolutionResaElementId",
                table: "OperativeParagraphs",
                column: "ResolutionResaElementId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationMember_OrganizationId",
                table: "OrganizationMember",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationMember_RoleOrganizationRoleId",
                table: "OrganizationMember",
                column: "RoleOrganizationRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationMember_UserId",
                table: "OrganizationMember",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationRoles_OrganizationId",
                table: "OrganizationRoles",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Participations_RoleId",
                table: "Participations",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Participations_UserId",
                table: "Participations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_AgendaItemId",
                table: "Petitions",
                column: "AgendaItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_PetitionTypeId",
                table: "Petitions",
                column: "PetitionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Petitions_SimulationUserId",
                table: "Petitions",
                column: "SimulationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PreambleParagraphs_ResaElementId",
                table: "PreambleParagraphs",
                column: "ResaElementId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectOrganizationOrganizationId",
                table: "Projects",
                column: "ProjectOrganizationOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionAuths_CommitteeId",
                table: "ResolutionAuths",
                column: "CommitteeId");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionAuths_CreationUserId",
                table: "ResolutionAuths",
                column: "CreationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionAuths_SimulationId",
                table: "ResolutionAuths",
                column: "SimulationId");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionSupporters_ResolutionResaElementId",
                table: "ResolutionSupporters",
                column: "ResolutionResaElementId");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionUsers_AuthResolutionId",
                table: "ResolutionUsers",
                column: "AuthResolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionUsers_UserId",
                table: "ResolutionUsers",
                column: "UserId");

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
                name: "IX_Settings_SetById",
                table: "Settings",
                column: "SetById");

            migrationBuilder.CreateIndex(
                name: "IX_SimulationHubConnections_UserSimulationUserId",
                table: "SimulationHubConnections",
                column: "UserSimulationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SimulationPetitionTypes_PetitionTypeId",
                table: "SimulationPetitionTypes",
                column: "PetitionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SimulationPetitionTypes_SimulationId",
                table: "SimulationPetitionTypes",
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
                name: "IX_SimulationStatuses_SimulationId",
                table: "SimulationStatuses",
                column: "SimulationId");

            migrationBuilder.CreateIndex(
                name: "IX_SimulationUser_RoleSimulationRoleId",
                table: "SimulationUser",
                column: "RoleSimulationRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SimulationUser_SimulationId",
                table: "SimulationUser",
                column: "SimulationId");

            migrationBuilder.CreateIndex(
                name: "IX_SimulationVotings_SimulationId",
                table: "SimulationVotings",
                column: "SimulationId");

            migrationBuilder.CreateIndex(
                name: "IX_Speakers_ListOfSpeakersId",
                table: "Speakers",
                column: "ListOfSpeakersId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPrivacySettings_UserRef",
                table: "UserPrivacySettings",
                column: "UserRef",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VotingSlots_UserSimulationUserId",
                table: "VotingSlots",
                column: "UserSimulationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_VotingSlots_VotingSimulationVotingId",
                table: "VotingSlots",
                column: "VotingSimulationVotingId");

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

            migrationBuilder.DropTable(
                name: "Amendments");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CommitteeSession");

            migrationBuilder.DropTable(
                name: "CommitteeTopic");

            migrationBuilder.DropTable(
                name: "OrganizationMember");

            migrationBuilder.DropTable(
                name: "Participations");

            migrationBuilder.DropTable(
                name: "Petitions");

            migrationBuilder.DropTable(
                name: "PreambleParagraphs");

            migrationBuilder.DropTable(
                name: "ResolutionSupporters");

            migrationBuilder.DropTable(
                name: "ResolutionUsers");

            migrationBuilder.DropTable(
                name: "RoleApplications");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "SimulationHubConnections");

            migrationBuilder.DropTable(
                name: "SimulationPetitionTypes");

            migrationBuilder.DropTable(
                name: "SimulationStatuses");

            migrationBuilder.DropTable(
                name: "Speakers");

            migrationBuilder.DropTable(
                name: "UserPrivacySettings");

            migrationBuilder.DropTable(
                name: "VotingSlots");

            migrationBuilder.DropTable(
                name: "OperativeParagraphs");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "OrganizationRoles");

            migrationBuilder.DropTable(
                name: "AgendaItems");

            migrationBuilder.DropTable(
                name: "ResolutionAuths");

            migrationBuilder.DropTable(
                name: "GroupedRoleApplications");

            migrationBuilder.DropTable(
                name: "PetitionTypes");

            migrationBuilder.DropTable(
                name: "SimulationUser");

            migrationBuilder.DropTable(
                name: "SimulationVotings");

            migrationBuilder.DropTable(
                name: "Resolutions");

            migrationBuilder.DropTable(
                name: "SimulationRoles");

            migrationBuilder.DropTable(
                name: "Simulations");

            migrationBuilder.DropTable(
                name: "ListOfSpeakers");

            migrationBuilder.DropTable(
                name: "Committees");

            migrationBuilder.DropTable(
                name: "Conferences");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "GroupApplications");

            migrationBuilder.DropTable(
                name: "UserAuths");

            migrationBuilder.DropTable(
                name: "Organizations");

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
        }
    }
}
