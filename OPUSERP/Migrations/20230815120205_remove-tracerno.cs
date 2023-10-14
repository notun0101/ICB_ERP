using Microsoft.EntityFrameworkCore.Migrations;

namespace OPUSERP.Migrations
{
    public partial class removetracerno : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TracerNo",
                schema: "Payroll",
                table: "StaffLoanLog");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TracerNo",
                schema: "Payroll",
                table: "StaffLoanLog",
                nullable: true);
        }
    }
}
