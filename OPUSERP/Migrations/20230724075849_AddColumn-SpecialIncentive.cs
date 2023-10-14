using Microsoft.EntityFrameworkCore.Migrations;

namespace OPUSERP.Migrations
{
    public partial class AddColumnSpecialIncentive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SpecialBenifit",
                schema: "Payroll",
                table: "AllSalarySheets",
                newName: "SpecialIncentive");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SpecialIncentive",
                schema: "Payroll",
                table: "AllSalarySheets",
                newName: "SpecialBenifit");
        }
    }
}
