using Microsoft.EntityFrameworkCore.Migrations;

namespace OPUSERP.Migrations
{
    public partial class AddingFieldToLoanCollection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeInfoId",
                schema: "PF",
                table: "LoanCollection",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IsPayrollEmi",
                schema: "PF",
                table: "LoanCollection",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "note",
                schema: "PF",
                table: "LoanCollection",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoanCollection_EmployeeInfoId",
                schema: "PF",
                table: "LoanCollection",
                column: "EmployeeInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanCollection_EmployeeInfo_EmployeeInfoId",
                schema: "PF",
                table: "LoanCollection",
                column: "EmployeeInfoId",
                principalSchema: "HR",
                principalTable: "EmployeeInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanCollection_EmployeeInfo_EmployeeInfoId",
                schema: "PF",
                table: "LoanCollection");

            migrationBuilder.DropIndex(
                name: "IX_LoanCollection_EmployeeInfoId",
                schema: "PF",
                table: "LoanCollection");

            migrationBuilder.DropColumn(
                name: "EmployeeInfoId",
                schema: "PF",
                table: "LoanCollection");

            migrationBuilder.DropColumn(
                name: "IsPayrollEmi",
                schema: "PF",
                table: "LoanCollection");

            migrationBuilder.DropColumn(
                name: "note",
                schema: "PF",
                table: "LoanCollection");
        }
    }
}
