using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OPUSERP.Migrations
{
    public partial class AddingTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PFInterestDistributionMaster",
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
                    processBy = table.Column<string>(nullable: true),
                    processDate = table.Column<string>(nullable: true),
                    fromDate = table.Column<DateTime>(nullable: true),
                    toDate = table.Column<DateTime>(nullable: true),
                    salaryPeriodId = table.Column<int>(nullable: true),
                    totalContribution = table.Column<decimal>(nullable: true),
                    totaldistributedInterest = table.Column<decimal>(nullable: true),
                    status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PFInterestDistributionMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PFInterestDistributionMaster_SalaryPeriod_salaryPeriodId",
                        column: x => x.salaryPeriodId,
                        principalSchema: "Payroll",
                        principalTable: "SalaryPeriod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PFInterestDistributionDetails",
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
                    PFMemberId = table.Column<int>(nullable: true),
                    employeeId = table.Column<int>(nullable: true),
                    totalContribution = table.Column<decimal>(nullable: true),
                    distributedInterest = table.Column<decimal>(nullable: true),
                    balance = table.Column<decimal>(nullable: true),
                    distributionMasterId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PFInterestDistributionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PFInterestDistributionDetails_PFMemberInfo_PFMemberId",
                        column: x => x.PFMemberId,
                        principalSchema: "PF",
                        principalTable: "PFMemberInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PFInterestDistributionDetails_PFInterestDistributionMaster_distributionMasterId",
                        column: x => x.distributionMasterId,
                        principalSchema: "PF",
                        principalTable: "PFInterestDistributionMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PFInterestDistributionDetails_EmployeeInfo_employeeId",
                        column: x => x.employeeId,
                        principalSchema: "HR",
                        principalTable: "EmployeeInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PFInterestDistributionDetails_PFMemberId",
                schema: "PF",
                table: "PFInterestDistributionDetails",
                column: "PFMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_PFInterestDistributionDetails_distributionMasterId",
                schema: "PF",
                table: "PFInterestDistributionDetails",
                column: "distributionMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_PFInterestDistributionDetails_employeeId",
                schema: "PF",
                table: "PFInterestDistributionDetails",
                column: "employeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PFInterestDistributionMaster_salaryPeriodId",
                schema: "PF",
                table: "PFInterestDistributionMaster",
                column: "salaryPeriodId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PFInterestDistributionDetails",
                schema: "PF");

            migrationBuilder.DropTable(
                name: "PFInterestDistributionMaster",
                schema: "PF");
        }
    }
}
