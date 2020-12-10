using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace SmartSaccos.persistence.Migrations
{
    public partial class Ringinenonsense : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HomeAddressId",
                table: "Members",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LearntAboutUs",
                table: "Members",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NearestTown",
                table: "Members",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NextOfKin",
                table: "Members",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NokContacts",
                table: "Members",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NokIsMinor",
                table: "Members",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NokRelation",
                table: "Members",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Occupation",
                table: "Members",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OccupationType",
                table: "Members",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PermanentAddressId",
                table: "Members",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SignatureId",
                table: "Members",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Members",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MemberAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UpdateCode = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    Village = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    District = table.Column<string>(nullable: true),
                    County = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberAddresses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Members_HomeAddressId",
                table: "Members",
                column: "HomeAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_PermanentAddressId",
                table: "Members",
                column: "PermanentAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_MemberAddresses_HomeAddressId",
                table: "Members",
                column: "HomeAddressId",
                principalTable: "MemberAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_MemberAddresses_PermanentAddressId",
                table: "Members",
                column: "PermanentAddressId",
                principalTable: "MemberAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_MemberAddresses_HomeAddressId",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_MemberAddresses_PermanentAddressId",
                table: "Members");

            migrationBuilder.DropTable(
                name: "MemberAddresses");

            migrationBuilder.DropIndex(
                name: "IX_Members_HomeAddressId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_PermanentAddressId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "HomeAddressId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "LearntAboutUs",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "NearestTown",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "NextOfKin",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "NokContacts",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "NokIsMinor",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "NokRelation",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Occupation",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "OccupationType",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "PermanentAddressId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "SignatureId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Members");
        }
    }
}
