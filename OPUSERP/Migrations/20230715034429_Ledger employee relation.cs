using Microsoft.EntityFrameworkCore.Migrations;

namespace OPUSERP.Migrations
{
    public partial class Ledgeremployeerelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                schema: "ACCOUNT",
                table: "Ledger",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ledger_EmployeeId",
                schema: "ACCOUNT",
                table: "Ledger",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ledger_EmployeeInfo_EmployeeId",
                schema: "ACCOUNT",
                table: "Ledger",
                column: "EmployeeId",
                principalSchema: "HR",
                principalTable: "EmployeeInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ledger_EmployeeInfo_EmployeeId",
                schema: "ACCOUNT",
                table: "Ledger");

            migrationBuilder.DropIndex(
                name: "IX_Ledger_EmployeeId",
                schema: "ACCOUNT",
                table: "Ledger");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                schema: "ACCOUNT",
                table: "Ledger");
        }
    }
}
