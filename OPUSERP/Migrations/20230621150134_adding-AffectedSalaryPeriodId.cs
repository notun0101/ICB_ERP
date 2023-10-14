using Microsoft.EntityFrameworkCore.Migrations;

namespace OPUSERP.Migrations
{
    public partial class addingAffectedSalaryPeriodId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AffectedSalaryPeriodId",
                schema: "Payroll",
                table: "SalaryPeriod",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AffectedSalaryPeriodId",
                schema: "Payroll",
                table: "SalaryPeriod");
        }
    }
}
