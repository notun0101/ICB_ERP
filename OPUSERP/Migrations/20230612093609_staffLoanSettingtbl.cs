using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OPUSERP.Migrations
{
    public partial class staffLoanSettingtbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsComplete",
                schema: "Payroll",
                table: "StaffLoan",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StaffLoanSetting",
                schema: "Payroll",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    isDelete = table.Column<int>(nullable: true),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: true),
                    createdBy = table.Column<string>(maxLength: 250, nullable: true),
                    updatedBy = table.Column<string>(maxLength: 250, nullable: true),
                    SalaryHeadId = table.Column<int>(nullable: true),
                    InstallmentAmount = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffLoanSetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffLoanSetting_SalaryHead_SalaryHeadId",
                        column: x => x.SalaryHeadId,
                        principalSchema: "Payroll",
                        principalTable: "SalaryHead",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StaffLoanSetting_SalaryHeadId",
                schema: "Payroll",
                table: "StaffLoanSetting",
                column: "SalaryHeadId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StaffLoanSetting",
                schema: "Payroll");

            migrationBuilder.DropColumn(
                name: "IsComplete",
                schema: "Payroll",
                table: "StaffLoan");
        }
    }
}
