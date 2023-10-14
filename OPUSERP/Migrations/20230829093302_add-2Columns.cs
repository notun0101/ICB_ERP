using Microsoft.EntityFrameworkCore.Migrations;

namespace OPUSERP.Migrations
{
    public partial class add2Columns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Signatory_EmployeeInfo_EmployeeInfoId",
                schema: "HR",
                table: "Signatory");

            migrationBuilder.DropColumn(
                name: "SignatoryId",
                schema: "HR",
                table: "Signatory");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeInfoId",
                schema: "PF",
                table: "PFContribution",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SalaryPeriodId",
                schema: "PF",
                table: "PFContribution",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeInfoId",
                schema: "HR",
                table: "Signatory",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PFContribution_EmployeeInfoId",
                schema: "PF",
                table: "PFContribution",
                column: "EmployeeInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_PFContribution_SalaryPeriodId",
                schema: "PF",
                table: "PFContribution",
                column: "SalaryPeriodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Signatory_EmployeeInfo_EmployeeInfoId",
                schema: "HR",
                table: "Signatory",
                column: "EmployeeInfoId",
                principalSchema: "HR",
                principalTable: "EmployeeInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_Signatory_EmployeeInfo_EmployeeInfoId",
                schema: "HR",
                table: "Signatory");

            migrationBuilder.DropForeignKey(
                name: "FK_PFContribution_EmployeeInfo_EmployeeInfoId",
                schema: "PF",
                table: "PFContribution");

            migrationBuilder.DropForeignKey(
                name: "FK_PFContribution_SalaryPeriod_SalaryPeriodId",
                schema: "PF",
                table: "PFContribution");

            migrationBuilder.DropIndex(
                name: "IX_PFContribution_EmployeeInfoId",
                schema: "PF",
                table: "PFContribution");

            migrationBuilder.DropIndex(
                name: "IX_PFContribution_SalaryPeriodId",
                schema: "PF",
                table: "PFContribution");

            migrationBuilder.DropColumn(
                name: "EmployeeInfoId",
                schema: "PF",
                table: "PFContribution");

            migrationBuilder.DropColumn(
                name: "SalaryPeriodId",
                schema: "PF",
                table: "PFContribution");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeInfoId",
                schema: "HR",
                table: "Signatory",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "SignatoryId",
                schema: "HR",
                table: "Signatory",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Signatory_EmployeeInfo_EmployeeInfoId",
                schema: "HR",
                table: "Signatory",
                column: "EmployeeInfoId",
                principalSchema: "HR",
                principalTable: "EmployeeInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
