using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OPUSERP.Migrations
{
    public partial class pfyearadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PFYearId",
                schema: "Payroll",
                table: "SalaryPeriod",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PFYear",
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
                    YearName = table.Column<string>(maxLength: 100, nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    YearStatus = table.Column<string>(nullable: true),
                    LockDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PFYear", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalaryPeriod_PFYearId",
                schema: "Payroll",
                table: "SalaryPeriod",
                column: "PFYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalaryPeriod_PFYear_PFYearId",
                schema: "Payroll",
                table: "SalaryPeriod",
                column: "PFYearId",
                principalSchema: "PF",
                principalTable: "PFYear",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalaryPeriod_PFYear_PFYearId",
                schema: "Payroll",
                table: "SalaryPeriod");

            migrationBuilder.DropTable(
                name: "PFYear",
                schema: "PF");

            migrationBuilder.DropIndex(
                name: "IX_SalaryPeriod_PFYearId",
                schema: "Payroll",
                table: "SalaryPeriod");

            migrationBuilder.DropColumn(
                name: "PFYearId",
                schema: "Payroll",
                table: "SalaryPeriod");
        }
    }
}
