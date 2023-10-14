using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OPUSERP.Migrations
{
    public partial class addcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SpecialBenifit",
                schema: "Payroll",
                table: "AllSalarySheets",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BonusPeriodLock",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    isDelete = table.Column<int>(nullable: true),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: true),
                    createdBy = table.Column<string>(maxLength: 250, nullable: true),
                    updatedBy = table.Column<string>(maxLength: 250, nullable: true),
                    SalaryYearId = table.Column<int>(nullable: false),
                    BonusTypeId = table.Column<int>(nullable: true),
                    PeriodId = table.Column<int>(nullable: false),
                    PeriodName = table.Column<string>(maxLength: 100, nullable: true),
                    LockLabel = table.Column<int>(nullable: true),
                    LockBy = table.Column<string>(maxLength: 100, nullable: true),
                    LockDate = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<int>(nullable: true),
                    CreateAt = table.Column<DateTime>(nullable: true),
                    UpdateBy = table.Column<int>(nullable: true),
                    UpdateAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonusPeriodLock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BonusPeriodLock_BonusType_BonusTypeId",
                        column: x => x.BonusTypeId,
                        principalSchema: "Payroll",
                        principalTable: "BonusType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BonusPeriodLock_SalaryYear_SalaryYearId",
                        column: x => x.SalaryYearId,
                        principalSchema: "Payroll",
                        principalTable: "SalaryYear",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BonusPeriodLock_BonusTypeId",
                table: "BonusPeriodLock",
                column: "BonusTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BonusPeriodLock_SalaryYearId",
                table: "BonusPeriodLock",
                column: "SalaryYearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BonusPeriodLock");

            migrationBuilder.DropColumn(
                name: "SpecialBenifit",
                schema: "Payroll",
                table: "AllSalarySheets");
        }
    }
}
