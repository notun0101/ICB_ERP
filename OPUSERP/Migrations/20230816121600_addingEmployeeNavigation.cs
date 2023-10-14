using Microsoft.EntityFrameworkCore.Migrations;

namespace OPUSERP.Migrations
{
    public partial class addingEmployeeNavigation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SignatoryId",
                schema: "HR",
                table: "Signatory",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeInfoId",
                schema: "HR",
                table: "Signatory",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Signatory_EmployeeInfoId",
                schema: "HR",
                table: "Signatory",
                column: "EmployeeInfoId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Signatory_EmployeeInfo_EmployeeInfoId",
                schema: "HR",
                table: "Signatory");

            migrationBuilder.DropIndex(
                name: "IX_Signatory_EmployeeInfoId",
                schema: "HR",
                table: "Signatory");

            migrationBuilder.DropColumn(
                name: "EmployeeInfoId",
                schema: "HR",
                table: "Signatory");

            migrationBuilder.AlterColumn<string>(
                name: "SignatoryId",
                schema: "HR",
                table: "Signatory",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
