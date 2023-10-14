using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OPUSERP.Migrations
{
    public partial class pfmemberTBlUPdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PFMemberInfo_Department_departmentId",
                schema: "PF",
                table: "PFMemberInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_PFMemberInfo_Designation_designationId",
                schema: "PF",
                table: "PFMemberInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_PFMemberInfo_ServiceStatus_employeeStatusId",
                schema: "PF",
                table: "PFMemberInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_PFMemberInfo_EmployeeType_memberTypeId",
                schema: "PF",
                table: "PFMemberInfo");

            migrationBuilder.DropIndex(
                name: "IX_PFMemberInfo_departmentId",
                schema: "PF",
                table: "PFMemberInfo");

            migrationBuilder.DropIndex(
                name: "IX_PFMemberInfo_designationId",
                schema: "PF",
                table: "PFMemberInfo");

            migrationBuilder.DropIndex(
                name: "IX_PFMemberInfo_employeeStatusId",
                schema: "PF",
                table: "PFMemberInfo");

            migrationBuilder.DropIndex(
                name: "IX_PFMemberInfo_memberTypeId",
                schema: "PF",
                table: "PFMemberInfo");

            migrationBuilder.DropColumn(
                name: "dateOfBirth",
                schema: "PF",
                table: "PFMemberInfo");

            migrationBuilder.DropColumn(
                name: "departmentId",
                schema: "PF",
                table: "PFMemberInfo");

            migrationBuilder.DropColumn(
                name: "designationId",
                schema: "PF",
                table: "PFMemberInfo");

            migrationBuilder.DropColumn(
                name: "email",
                schema: "PF",
                table: "PFMemberInfo");

            migrationBuilder.DropColumn(
                name: "employeeCode",
                schema: "PF",
                table: "PFMemberInfo");

            migrationBuilder.DropColumn(
                name: "employeeStatusId",
                schema: "PF",
                table: "PFMemberInfo");

            migrationBuilder.DropColumn(
                name: "memberTypeId",
                schema: "PF",
                table: "PFMemberInfo");

            migrationBuilder.DropColumn(
                name: "mobile",
                schema: "PF",
                table: "PFMemberInfo");

            migrationBuilder.DropColumn(
                name: "nid",
                schema: "PF",
                table: "PFMemberInfo");

            migrationBuilder.RenameColumn(
                name: "dateOfJoining",
                schema: "PF",
                table: "PFMemberInfo",
                newName: "approveDate");

            migrationBuilder.AlterColumn<string>(
                name: "memberName",
                schema: "PF",
                table: "PFMemberInfo",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 250,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "approveDate",
                schema: "PF",
                table: "PFMemberInfo",
                newName: "dateOfJoining");

            migrationBuilder.AlterColumn<string>(
                name: "memberName",
                schema: "PF",
                table: "PFMemberInfo",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "dateOfBirth",
                schema: "PF",
                table: "PFMemberInfo",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "departmentId",
                schema: "PF",
                table: "PFMemberInfo",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "designationId",
                schema: "PF",
                table: "PFMemberInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "email",
                schema: "PF",
                table: "PFMemberInfo",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "employeeCode",
                schema: "PF",
                table: "PFMemberInfo",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "employeeStatusId",
                schema: "PF",
                table: "PFMemberInfo",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "memberTypeId",
                schema: "PF",
                table: "PFMemberInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "mobile",
                schema: "PF",
                table: "PFMemberInfo",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nid",
                schema: "PF",
                table: "PFMemberInfo",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PFMemberInfo_departmentId",
                schema: "PF",
                table: "PFMemberInfo",
                column: "departmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PFMemberInfo_designationId",
                schema: "PF",
                table: "PFMemberInfo",
                column: "designationId");

            migrationBuilder.CreateIndex(
                name: "IX_PFMemberInfo_employeeStatusId",
                schema: "PF",
                table: "PFMemberInfo",
                column: "employeeStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PFMemberInfo_memberTypeId",
                schema: "PF",
                table: "PFMemberInfo",
                column: "memberTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PFMemberInfo_Department_departmentId",
                schema: "PF",
                table: "PFMemberInfo",
                column: "departmentId",
                principalSchema: "HR",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PFMemberInfo_Designation_designationId",
                schema: "PF",
                table: "PFMemberInfo",
                column: "designationId",
                principalSchema: "HR",
                principalTable: "Designation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PFMemberInfo_ServiceStatus_employeeStatusId",
                schema: "PF",
                table: "PFMemberInfo",
                column: "employeeStatusId",
                principalSchema: "HR",
                principalTable: "ServiceStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PFMemberInfo_EmployeeType_memberTypeId",
                schema: "PF",
                table: "PFMemberInfo",
                column: "memberTypeId",
                principalSchema: "HR",
                principalTable: "EmployeeType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
