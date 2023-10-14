using Microsoft.EntityFrameworkCore.Migrations;

namespace OPUSERP.Migrations
{
    public partial class removingcolumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SignatoryDesignation",
                schema: "HR",
                table: "Signatory");

            migrationBuilder.DropColumn(
                name: "SignatoryName",
                schema: "HR",
                table: "Signatory");

            migrationBuilder.DropColumn(
                name: "SignatoryPhone",
                schema: "HR",
                table: "Signatory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SignatoryDesignation",
                schema: "HR",
                table: "Signatory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SignatoryName",
                schema: "HR",
                table: "Signatory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SignatoryPhone",
                schema: "HR",
                table: "Signatory",
                nullable: true);
        }
    }
}
