using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartSaccos.persistence.Migrations
{
    public partial class membersnumbers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MemberNumber",
                table: "CompanyDefaults",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MemberNumber",
                table: "CompanyDefaults");
        }
    }
}
