using Microsoft.EntityFrameworkCore.Migrations;

namespace OPUSERP.Migrations
{
    public partial class addofficePhoneBn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "telephoneOfficeBn",
                schema: "HR",
                table: "EmployeeInfo",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "telephoneOfficeBn",
                schema: "HR",
                table: "EmployeeInfo");
        }
    }
}
