using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartSaccos.persistence.Migrations
{
    public partial class memberpassport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PassportCopyId",
                table: "Members",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PassportCopyId",
                table: "Members");
        }
    }
}
