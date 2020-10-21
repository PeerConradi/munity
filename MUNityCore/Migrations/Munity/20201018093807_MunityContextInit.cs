using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MUNityCore.Migrations.Munity
{
    public partial class MunityContextInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "ResolutionUsers",
                columns: table => new
                {
                    ResolutionUserId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CoreUserId = table.Column<int>(nullable: false),
                    Username = table.Column<string>(maxLength: 100, nullable: true),
                    ForeName = table.Column<string>(maxLength: 150, nullable: true),
                    LastName = table.Column<string>(maxLength: 150, nullable: true),
                    CanRead = table.Column<bool>(nullable: false),
                    CanWrite = table.Column<bool>(nullable: false),
                    CanAddUsers = table.Column<bool>(nullable: false),
                    AuthResolutionId = table.Column<string>(nullable: true)
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
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResolutionUsers_AuthResolutionId",
                table: "ResolutionUsers",
                column: "AuthResolutionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResolutionUsers");

            migrationBuilder.DropTable(
                name: "ResolutionAuths");
        }
    }
}
