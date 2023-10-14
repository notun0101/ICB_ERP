using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OPUSERP.Migrations
{
    public partial class updatecontributionTBL : Migration
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

            migrationBuilder.DropColumn(
                name: "contributionMonth",
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

            migrationBuilder.AddColumn<string>(
                name: "Particulars",
                schema: "PF",
                table: "PFContribution",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "credit",
                schema: "PF",
                table: "PFContribution",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "debit",
                schema: "PF",
                table: "PFContribution",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "isJournal",
                schema: "PF",
                table: "PFContribution",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "narration",
                schema: "PF",
                table: "PFContribution",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "profitOnCompanyContribution",
                schema: "PF",
                table: "PFContribution",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "profitOnEmployeeContribution",
                schema: "PF",
                table: "PFContribution",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "totalProfit",
                schema: "PF",
                table: "PFContribution",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "withdrawn",
                schema: "PF",
                table: "PFContribution",
                nullable: true);

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

            migrationBuilder.DropColumn(
                name: "Particulars",
                schema: "PF",
                table: "PFContribution");

            migrationBuilder.DropColumn(
                name: "credit",
                schema: "PF",
                table: "PFContribution");

            migrationBuilder.DropColumn(
                name: "debit",
                schema: "PF",
                table: "PFContribution");

            migrationBuilder.DropColumn(
                name: "isJournal",
                schema: "PF",
                table: "PFContribution");

            migrationBuilder.DropColumn(
                name: "narration",
                schema: "PF",
                table: "PFContribution");

            migrationBuilder.DropColumn(
                name: "profitOnCompanyContribution",
                schema: "PF",
                table: "PFContribution");

            migrationBuilder.DropColumn(
                name: "profitOnEmployeeContribution",
                schema: "PF",
                table: "PFContribution");

            migrationBuilder.DropColumn(
                name: "totalProfit",
                schema: "PF",
                table: "PFContribution");

            migrationBuilder.DropColumn(
                name: "withdrawn",
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

            migrationBuilder.AddColumn<DateTime>(
                name: "contributionMonth",
                schema: "PF",
                table: "PFContribution",
                nullable: true);

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
    }
}
