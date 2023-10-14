using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OPUSERP.Migrations
{
    public partial class AddingPFTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "expectedDate",
                schema: "PF",
                table: "PFLoanManagement",
                newName: "disbursementDate");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeInfoId",
                schema: "PF",
                table: "PFLoanManagement",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InterestProvision",
                schema: "PF",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    isDelete = table.Column<int>(nullable: true),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: true),
                    createdBy = table.Column<string>(maxLength: 250, nullable: true),
                    updatedBy = table.Column<string>(maxLength: 250, nullable: true),
                    pfmemberId = table.Column<int>(nullable: true),
                    loanManagementId = table.Column<int>(nullable: true),
                    EmployeeInfoId = table.Column<int>(nullable: true),
                    SalaryPeriodId = table.Column<int>(nullable: true),
                    InterestAmount = table.Column<decimal>(nullable: true),
                    TransactionDate = table.Column<DateTime>(nullable: true),
                    TransactionBehaviour = table.Column<decimal>(nullable: true),
                    note = table.Column<string>(nullable: true),
                    branchId = table.Column<int>(nullable: true),
                    isJournal = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestProvision", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterestProvision_EmployeeInfo_EmployeeInfoId",
                        column: x => x.EmployeeInfoId,
                        principalSchema: "HR",
                        principalTable: "EmployeeInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InterestProvision_SalaryPeriod_SalaryPeriodId",
                        column: x => x.SalaryPeriodId,
                        principalSchema: "Payroll",
                        principalTable: "SalaryPeriod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InterestProvision_PFLoanManagement_loanManagementId",
                        column: x => x.loanManagementId,
                        principalSchema: "PF",
                        principalTable: "PFLoanManagement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InterestProvision_PFMemberInfo_pfmemberId",
                        column: x => x.pfmemberId,
                        principalSchema: "PF",
                        principalTable: "PFMemberInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoanCollection",
                schema: "PF",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    isDelete = table.Column<int>(nullable: true),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: true),
                    createdBy = table.Column<string>(maxLength: 250, nullable: true),
                    updatedBy = table.Column<string>(maxLength: 250, nullable: true),
                    pfmemberId = table.Column<int>(nullable: true),
                    loanManagementId = table.Column<int>(nullable: true),
                    SalaryPeriodId = table.Column<int>(nullable: true),
                    collectionAmount = table.Column<decimal>(nullable: true),
                    collectionDate = table.Column<DateTime>(nullable: true),
                    branchId = table.Column<int>(nullable: true),
                    isJournal = table.Column<int>(nullable: true),
                    duration = table.Column<int>(nullable: true),
                    particular = table.Column<string>(nullable: true),
                    debit = table.Column<decimal>(nullable: true),
                    credit = table.Column<decimal>(nullable: true),
                    principal = table.Column<decimal>(nullable: true),
                    interestCharge = table.Column<decimal>(nullable: true),
                    interestPer = table.Column<decimal>(nullable: true),
                    interest = table.Column<decimal>(nullable: true),
                    isProcessed = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanCollection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanCollection_SalaryPeriod_SalaryPeriodId",
                        column: x => x.SalaryPeriodId,
                        principalSchema: "Payroll",
                        principalTable: "SalaryPeriod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoanCollection_PFLoanManagement_loanManagementId",
                        column: x => x.loanManagementId,
                        principalSchema: "PF",
                        principalTable: "PFLoanManagement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoanCollection_PFMemberInfo_pfmemberId",
                        column: x => x.pfmemberId,
                        principalSchema: "PF",
                        principalTable: "PFMemberInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PFLoanManagement_EmployeeInfoId",
                schema: "PF",
                table: "PFLoanManagement",
                column: "EmployeeInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_InterestProvision_EmployeeInfoId",
                schema: "PF",
                table: "InterestProvision",
                column: "EmployeeInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_InterestProvision_SalaryPeriodId",
                schema: "PF",
                table: "InterestProvision",
                column: "SalaryPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_InterestProvision_loanManagementId",
                schema: "PF",
                table: "InterestProvision",
                column: "loanManagementId");

            migrationBuilder.CreateIndex(
                name: "IX_InterestProvision_pfmemberId",
                schema: "PF",
                table: "InterestProvision",
                column: "pfmemberId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanCollection_SalaryPeriodId",
                schema: "PF",
                table: "LoanCollection",
                column: "SalaryPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanCollection_loanManagementId",
                schema: "PF",
                table: "LoanCollection",
                column: "loanManagementId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanCollection_pfmemberId",
                schema: "PF",
                table: "LoanCollection",
                column: "pfmemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_PFLoanManagement_EmployeeInfo_EmployeeInfoId",
                schema: "PF",
                table: "PFLoanManagement",
                column: "EmployeeInfoId",
                principalSchema: "HR",
                principalTable: "EmployeeInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PFLoanManagement_EmployeeInfo_EmployeeInfoId",
                schema: "PF",
                table: "PFLoanManagement");

            migrationBuilder.DropTable(
                name: "InterestProvision",
                schema: "PF");

            migrationBuilder.DropTable(
                name: "LoanCollection",
                schema: "PF");

            migrationBuilder.DropIndex(
                name: "IX_PFLoanManagement_EmployeeInfoId",
                schema: "PF",
                table: "PFLoanManagement");

            migrationBuilder.DropColumn(
                name: "EmployeeInfoId",
                schema: "PF",
                table: "PFLoanManagement");

            migrationBuilder.RenameColumn(
                name: "disbursementDate",
                schema: "PF",
                table: "PFLoanManagement",
                newName: "expectedDate");
        }
    }
}
