using Microsoft.EntityFrameworkCore.Migrations;

namespace OPUSERP.Migrations
{
    public partial class OmitNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PFContribution_EmployeeInfo_EmployeeInfoId",
                schema: "PF",
                table: "PFContribution");

            migrationBuilder.DropForeignKey(
                name: "FK_PFContribution_SalaryPeriod_SalaryPeriodId",
                schema: "PF",
                table: "PFContribution");

            migrationBuilder.AlterColumn<int>(
                name: "SalaryPeriodId",
                schema: "PF",
                table: "PFContribution",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeInfoId",
                schema: "PF",
                table: "PFContribution",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PFContribution_EmployeeInfo_EmployeeInfoId",
                schema: "PF",
                table: "PFContribution",
                column: "EmployeeInfoId",
                principalSchema: "HR",
                principalTable: "EmployeeInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PFContribution_SalaryPeriod_SalaryPeriodId",
                schema: "PF",
                table: "PFContribution",
                column: "SalaryPeriodId",
                principalSchema: "Payroll",
                principalTable: "SalaryPeriod",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PFContribution_EmployeeInfo_EmployeeInfoId",
                schema: "PF",
                table: "PFContribution");

            migrationBuilder.DropForeignKey(
                name: "FK_PFContribution_SalaryPeriod_SalaryPeriodId",
                schema: "PF",
                table: "PFContribution");

            migrationBuilder.AlterColumn<int>(
                name: "SalaryPeriodId",
                schema: "PF",
                table: "PFContribution",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeInfoId",
                schema: "PF",
                table: "PFContribution",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_PFContribution_EmployeeInfo_EmployeeInfoId",
                schema: "PF",
                table: "PFContribution",
                column: "EmployeeInfoId",
                principalSchema: "HR",
                principalTable: "EmployeeInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PFContribution_SalaryPeriod_SalaryPeriodId",
                schema: "PF",
                table: "PFContribution",
                column: "SalaryPeriodId",
                principalSchema: "Payroll",
                principalTable: "SalaryPeriod",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
