using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MUNityCore.Migrations
{
    public partial class AddedPresentCheckModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupApplications_AbstractRole_RoleId",
                table: "GroupApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_Participations_AbstractRole_RoleId",
                table: "Participations");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleApplications_AbstractRole_RoleId",
                table: "RoleApplications");

            migrationBuilder.DropTable(
                name: "AbstractRole");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TeamRoleGroupTimestamp",
                table: "TeamRoleGroups",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RoleAuthTimestamp",
                table: "RoleAuths",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RoleApplicationTimestamp",
                table: "RoleApplications",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ProjectTimestamp",
                table: "Projects",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ParticipationTimestamp",
                table: "Participations",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrganizationTimestamp",
                table: "Organizations",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrganizationMemberTimestamp",
                table: "OrganizationMember",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "GroupedRoleApplicationTimestamp",
                table: "GroupedRoleApplications",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "GroupApplicationTimestamp",
                table: "GroupApplications",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CountryTimestamp",
                table: "Countries",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ConferenceTimestamp",
                table: "Conferences",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CommitteeTopicTimestamp",
                table: "CommitteeTopic",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CommitteeTimestamp",
                table: "Committees",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UserTimestamp",
                table: "AspNetUsers",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.CreateTable(
                name: "AbstractConferenceRole",
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
                    ConferenceSecretaryGeneralRole_Title = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: true),
                    ParentTeamRoleRoleId = table.Column<int>(type: "int", nullable: true),
                    TeamRoleLevel = table.Column<int>(type: "int", nullable: true),
                    TeamRoleGroupId = table.Column<int>(type: "int", nullable: true),
                    Organization = table.Column<string>(type: "varchar(100) CHARACTER SET utf8mb4", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbstractConferenceRole", x => x.RoleId);
                    table.ForeignKey(
                        name: "FK_AbstractConferenceRole_AbstractConferenceRole_ParentTeamRole~",
                        column: x => x.ParentTeamRoleRoleId,
                        principalTable: "AbstractConferenceRole",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbstractConferenceRole_Committees_CommitteeId",
                        column: x => x.CommitteeId,
                        principalTable: "Committees",
                        principalColumn: "CommitteeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbstractConferenceRole_Conferences_ConferenceId",
                        column: x => x.ConferenceId,
                        principalTable: "Conferences",
                        principalColumn: "ConferenceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbstractConferenceRole_Countries_DelegateStateCountryId",
                        column: x => x.DelegateStateCountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbstractConferenceRole_Delegation_DelegationId",
                        column: x => x.DelegationId,
                        principalTable: "Delegation",
                        principalColumn: "DelegationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbstractConferenceRole_RoleAuths_RoleAuthId",
                        column: x => x.RoleAuthId,
                        principalTable: "RoleAuths",
                        principalColumn: "RoleAuthId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbstractConferenceRole_TeamRoleGroups_TeamRoleGroupId",
                        column: x => x.TeamRoleGroupId,
                        principalTable: "TeamRoleGroups",
                        principalColumn: "TeamRoleGroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PresentChecks",
                columns: table => new
                {
                    SimulationPresentsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SimulationId = table.Column<int>(type: "int", nullable: true),
                    CheckedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresentChecks", x => x.SimulationPresentsId);
                    table.ForeignKey(
                        name: "FK_PresentChecks_Simulations_SimulationId",
                        column: x => x.SimulationId,
                        principalTable: "Simulations",
                        principalColumn: "SimulationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PresentStates",
                columns: table => new
                {
                    PresentsStateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SimulationPresentsId = table.Column<int>(type: "int", nullable: true),
                    SimulationUserId = table.Column<int>(type: "int", nullable: true),
                    StatePresentsStateId = table.Column<int>(type: "int", nullable: true),
                    StateValue = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    RegistertTimestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresentStates", x => x.PresentsStateId);
                    table.ForeignKey(
                        name: "FK_PresentStates_PresentChecks_SimulationPresentsId",
                        column: x => x.SimulationPresentsId,
                        principalTable: "PresentChecks",
                        principalColumn: "SimulationPresentsId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PresentStates_PresentStates_StatePresentsStateId",
                        column: x => x.StatePresentsStateId,
                        principalTable: "PresentStates",
                        principalColumn: "PresentsStateId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PresentStates_SimulationUser_SimulationUserId",
                        column: x => x.SimulationUserId,
                        principalTable: "SimulationUser",
                        principalColumn: "SimulationUserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbstractConferenceRole_CommitteeId",
                table: "AbstractConferenceRole",
                column: "CommitteeId");

            migrationBuilder.CreateIndex(
                name: "IX_AbstractConferenceRole_ConferenceId",
                table: "AbstractConferenceRole",
                column: "ConferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_AbstractConferenceRole_DelegateStateCountryId",
                table: "AbstractConferenceRole",
                column: "DelegateStateCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_AbstractConferenceRole_DelegationId",
                table: "AbstractConferenceRole",
                column: "DelegationId");

            migrationBuilder.CreateIndex(
                name: "IX_AbstractConferenceRole_ParentTeamRoleRoleId",
                table: "AbstractConferenceRole",
                column: "ParentTeamRoleRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AbstractConferenceRole_RoleAuthId",
                table: "AbstractConferenceRole",
                column: "RoleAuthId");

            migrationBuilder.CreateIndex(
                name: "IX_AbstractConferenceRole_TeamRoleGroupId",
                table: "AbstractConferenceRole",
                column: "TeamRoleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentChecks_SimulationId",
                table: "PresentChecks",
                column: "SimulationId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentStates_SimulationPresentsId",
                table: "PresentStates",
                column: "SimulationPresentsId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentStates_SimulationUserId",
                table: "PresentStates",
                column: "SimulationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentStates_StatePresentsStateId",
                table: "PresentStates",
                column: "StatePresentsStateId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupApplications_AbstractConferenceRole_RoleId",
                table: "GroupApplications",
                column: "RoleId",
                principalTable: "AbstractConferenceRole",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Participations_AbstractConferenceRole_RoleId",
                table: "Participations",
                column: "RoleId",
                principalTable: "AbstractConferenceRole",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleApplications_AbstractConferenceRole_RoleId",
                table: "RoleApplications",
                column: "RoleId",
                principalTable: "AbstractConferenceRole",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupApplications_AbstractConferenceRole_RoleId",
                table: "GroupApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_Participations_AbstractConferenceRole_RoleId",
                table: "Participations");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleApplications_AbstractConferenceRole_RoleId",
                table: "RoleApplications");

            migrationBuilder.DropTable(
                name: "AbstractConferenceRole");

            migrationBuilder.DropTable(
                name: "PresentStates");

            migrationBuilder.DropTable(
                name: "PresentChecks");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TeamRoleGroupTimestamp",
                table: "TeamRoleGroups",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RoleAuthTimestamp",
                table: "RoleAuths",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RoleApplicationTimestamp",
                table: "RoleApplications",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ProjectTimestamp",
                table: "Projects",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ParticipationTimestamp",
                table: "Participations",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrganizationTimestamp",
                table: "Organizations",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrganizationMemberTimestamp",
                table: "OrganizationMember",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "GroupedRoleApplicationTimestamp",
                table: "GroupedRoleApplications",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "GroupApplicationTimestamp",
                table: "GroupApplications",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CountryTimestamp",
                table: "Countries",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ConferenceTimestamp",
                table: "Conferences",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CommitteeTopicTimestamp",
                table: "CommitteeTopic",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CommitteeTimestamp",
                table: "Committees",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UserTimestamp",
                table: "AspNetUsers",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.CreateTable(
                name: "AbstractRole",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AllowMultipleParticipations = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ApplicationState = table.Column<int>(type: "int", nullable: false),
                    ApplicationValue = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: true),
                    ConferenceId = table.Column<string>(type: "varchar(80) CHARACTER SET utf8mb4", nullable: true),
                    IconName = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: true),
                    RoleAuthId = table.Column<int>(type: "int", nullable: true),
                    RoleFullName = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: true),
                    RoleName = table.Column<string>(type: "varchar(150) CHARACTER SET utf8mb4", maxLength: 150, nullable: true),
                    RoleShort = table.Column<string>(type: "varchar(10) CHARACTER SET utf8mb4", maxLength: 10, nullable: true),
                    RoleTimestamp = table.Column<DateTime>(type: "timestamp(6)", rowVersion: true, nullable: true),
                    RoleType = table.Column<string>(type: "varchar(150) CHARACTER SET utf8mb4", maxLength: 150, nullable: false),
                    CommitteeId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    DelegateStateCountryId = table.Column<int>(type: "int", nullable: true),
                    DelegationId = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", nullable: true),
                    IsDelegationLeader = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Title = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Group = table.Column<int>(type: "int", nullable: true),
                    Leader = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    NgoName = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: true),
                    PressCategory = table.Column<int>(type: "int", nullable: true),
                    SecretaryGeneralRole_Title = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: true),
                    ParentTeamRoleRoleId = table.Column<int>(type: "int", nullable: true),
                    TeamRoleGroupId = table.Column<int>(type: "int", nullable: true),
                    TeamRoleLevel = table.Column<int>(type: "int", nullable: true),
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

            migrationBuilder.AddForeignKey(
                name: "FK_GroupApplications_AbstractRole_RoleId",
                table: "GroupApplications",
                column: "RoleId",
                principalTable: "AbstractRole",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Participations_AbstractRole_RoleId",
                table: "Participations",
                column: "RoleId",
                principalTable: "AbstractRole",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleApplications_AbstractRole_RoleId",
                table: "RoleApplications",
                column: "RoleId",
                principalTable: "AbstractRole",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
