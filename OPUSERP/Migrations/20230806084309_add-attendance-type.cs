using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OPUSERP.Migrations
{
    public partial class addattendancetype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttendanceTypeId",
                schema: "HR",
                table: "EmpManualAttendance",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AttendanceType",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    isDelete = table.Column<int>(nullable: true),
                    createdAt = table.Column<DateTime>(nullable: true),
                    updatedAt = table.Column<DateTime>(nullable: true),
                    createdBy = table.Column<string>(maxLength: 250, nullable: true),
                    updatedBy = table.Column<string>(maxLength: 250, nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmpManualAttendance_AttendanceTypeId",
                schema: "HR",
                table: "EmpManualAttendance",
                column: "AttendanceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmpManualAttendance_AttendanceType_AttendanceTypeId",
                schema: "HR",
                table: "EmpManualAttendance",
                column: "AttendanceTypeId",
                principalSchema: "HR",
                principalTable: "AttendanceType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmpManualAttendance_AttendanceType_AttendanceTypeId",
                schema: "HR",
                table: "EmpManualAttendance");

            migrationBuilder.DropTable(
                name: "AttendanceType",
                schema: "HR");

            migrationBuilder.DropIndex(
                name: "IX_EmpManualAttendance_AttendanceTypeId",
                schema: "HR",
                table: "EmpManualAttendance");

            migrationBuilder.DropColumn(
                name: "AttendanceTypeId",
                schema: "HR",
                table: "EmpManualAttendance");
        }
    }
}
