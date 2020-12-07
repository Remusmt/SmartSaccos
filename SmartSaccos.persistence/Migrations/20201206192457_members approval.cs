using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace SmartSaccos.persistence.Migrations
{
    public partial class membersapproval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MemberApprovals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UpdateCode = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ApplicationUserId = table.Column<int>(nullable: false),
                    MemberId = table.Column<int>(nullable: false),
                    ApprovalAction = table.Column<int>(nullable: false),
                    MessageToMember = table.Column<string>(nullable: true),
                    Comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberApprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberApprovals_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberApprovals_MemberId",
                table: "MemberApprovals",
                column: "MemberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberApprovals");
        }
    }
}
