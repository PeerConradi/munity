using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MUNityCore.Migrations
{
    public partial class AddedPetitionTypesandTemplating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PetitionTypeSimulation_PetitionTypes_PetitionTypeId",
                table: "PetitionTypeSimulation");

            migrationBuilder.DropForeignKey(
                name: "FK_PetitionTypeSimulation_Simulations_SimulationId",
                table: "PetitionTypeSimulation");

            migrationBuilder.DropForeignKey(
                name: "FK_SimulationRoles_PetitionTypeSimulation_PetitionTypeSimulatio~",
                table: "SimulationRoles");

            migrationBuilder.DropIndex(
                name: "IX_SimulationRoles_PetitionTypeSimulationId",
                table: "SimulationRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PetitionTypeSimulation",
                table: "PetitionTypeSimulation");

            migrationBuilder.DropColumn(
                name: "PetitionTypeSimulationId",
                table: "SimulationRoles");

            migrationBuilder.RenameTable(
                name: "PetitionTypeSimulation",
                newName: "SimulationPetitionTypes");

            migrationBuilder.RenameIndex(
                name: "IX_PetitionTypeSimulation_SimulationId",
                table: "SimulationPetitionTypes",
                newName: "IX_SimulationPetitionTypes_SimulationId");

            migrationBuilder.RenameIndex(
                name: "IX_PetitionTypeSimulation_PetitionTypeId",
                table: "SimulationPetitionTypes",
                newName: "IX_SimulationPetitionTypes_PetitionTypeId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UserTimestamp",
                table: "Users",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

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
                name: "RoleTimestamp",
                table: "AbstractRole",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AddColumn<bool>(
                name: "AllowChairs",
                table: "SimulationPetitionTypes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllowDelegates",
                table: "SimulationPetitionTypes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllowNgo",
                table: "SimulationPetitionTypes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllowSpectator",
                table: "SimulationPetitionTypes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SimulationPetitionTypes",
                table: "SimulationPetitionTypes",
                column: "PetitionTypeSimulationId");

            migrationBuilder.AddForeignKey(
                name: "FK_SimulationPetitionTypes_PetitionTypes_PetitionTypeId",
                table: "SimulationPetitionTypes",
                column: "PetitionTypeId",
                principalTable: "PetitionTypes",
                principalColumn: "PetitionTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SimulationPetitionTypes_Simulations_SimulationId",
                table: "SimulationPetitionTypes",
                column: "SimulationId",
                principalTable: "Simulations",
                principalColumn: "SimulationId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SimulationPetitionTypes_PetitionTypes_PetitionTypeId",
                table: "SimulationPetitionTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_SimulationPetitionTypes_Simulations_SimulationId",
                table: "SimulationPetitionTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SimulationPetitionTypes",
                table: "SimulationPetitionTypes");

            migrationBuilder.DropColumn(
                name: "AllowChairs",
                table: "SimulationPetitionTypes");

            migrationBuilder.DropColumn(
                name: "AllowDelegates",
                table: "SimulationPetitionTypes");

            migrationBuilder.DropColumn(
                name: "AllowNgo",
                table: "SimulationPetitionTypes");

            migrationBuilder.DropColumn(
                name: "AllowSpectator",
                table: "SimulationPetitionTypes");

            migrationBuilder.RenameTable(
                name: "SimulationPetitionTypes",
                newName: "PetitionTypeSimulation");

            migrationBuilder.RenameIndex(
                name: "IX_SimulationPetitionTypes_SimulationId",
                table: "PetitionTypeSimulation",
                newName: "IX_PetitionTypeSimulation_SimulationId");

            migrationBuilder.RenameIndex(
                name: "IX_SimulationPetitionTypes_PetitionTypeId",
                table: "PetitionTypeSimulation",
                newName: "IX_PetitionTypeSimulation_PetitionTypeId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UserTimestamp",
                table: "Users",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

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

            migrationBuilder.AddColumn<long>(
                name: "PetitionTypeSimulationId",
                table: "SimulationRoles",
                type: "bigint",
                nullable: true);

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
                name: "RoleTimestamp",
                table: "AbstractRole",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PetitionTypeSimulation",
                table: "PetitionTypeSimulation",
                column: "PetitionTypeSimulationId");

            migrationBuilder.CreateIndex(
                name: "IX_SimulationRoles_PetitionTypeSimulationId",
                table: "SimulationRoles",
                column: "PetitionTypeSimulationId");

            migrationBuilder.AddForeignKey(
                name: "FK_PetitionTypeSimulation_PetitionTypes_PetitionTypeId",
                table: "PetitionTypeSimulation",
                column: "PetitionTypeId",
                principalTable: "PetitionTypes",
                principalColumn: "PetitionTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PetitionTypeSimulation_Simulations_SimulationId",
                table: "PetitionTypeSimulation",
                column: "SimulationId",
                principalTable: "Simulations",
                principalColumn: "SimulationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SimulationRoles_PetitionTypeSimulation_PetitionTypeSimulatio~",
                table: "SimulationRoles",
                column: "PetitionTypeSimulationId",
                principalTable: "PetitionTypeSimulation",
                principalColumn: "PetitionTypeSimulationId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
