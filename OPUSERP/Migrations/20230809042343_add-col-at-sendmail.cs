using Microsoft.EntityFrameworkCore.Migrations;

namespace OPUSERP.Migrations
{
    public partial class addcolatsendmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "SendEmailLogStatus",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DesignationId",
                table: "SendEmailLogStatus",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PeriodId",
                table: "SendEmailLogStatus",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "SendEmailLogStatus");

            migrationBuilder.DropColumn(
                name: "DesignationId",
                table: "SendEmailLogStatus");

            migrationBuilder.DropColumn(
                name: "PeriodId",
                table: "SendEmailLogStatus");
        }
    }
}
