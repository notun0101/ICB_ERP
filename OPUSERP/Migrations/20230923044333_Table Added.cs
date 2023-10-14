using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OPUSERP.Migrations
{
    public partial class TableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PFLoanDisbursement",
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
                    EmployeeInfoId = table.Column<int>(nullable: true),
                    pfmemberId = table.Column<int>(nullable: true),
                    loanAmount = table.Column<decimal>(nullable: true),
                    installmentAmount = table.Column<decimal>(nullable: true),
                    tenure = table.Column<int>(nullable: true),
                    interestRate = table.Column<int>(nullable: true),
                    note = table.Column<string>(nullable: true),
                    expectedSettlementDate = table.Column<DateTime>(nullable: true),
                    expectedDate = table.Column<DateTime>(nullable: true),
                    applicationDate = table.Column<DateTime>(nullable: true),
                    approveStatus = table.Column<int>(nullable: true),
                    isActive = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PFLoanDisbursement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PFLoanDisbursement_EmployeeInfo_EmployeeInfoId",
                        column: x => x.EmployeeInfoId,
                        principalSchema: "HR",
                        principalTable: "EmployeeInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PFLoanDisbursement_PFMemberInfo_pfmemberId",
                        column: x => x.pfmemberId,
                        principalSchema: "PF",
                        principalTable: "PFMemberInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PFLoanDisbursement_EmployeeInfoId",
                schema: "PF",
                table: "PFLoanDisbursement",
                column: "EmployeeInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_PFLoanDisbursement_pfmemberId",
                schema: "PF",
                table: "PFLoanDisbursement",
                column: "pfmemberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PFLoanDisbursement",
                schema: "PF");
        }
    }
}
