using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OPUSERP.Migrations
{
    public partial class addingSignatoryEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Signatory",
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
                    DesignationId = table.Column<int>(nullable: false),
                    SignatoryId = table.Column<string>(nullable: true),
                    SignatoryName = table.Column<string>(nullable: true),
                    SignatoryDesignation = table.Column<string>(nullable: true),
                    SignatoryPhone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Signatory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Signatory_Designation_DesignationId",
                        column: x => x.DesignationId,
                        principalSchema: "HR",
                        principalTable: "Designation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Signatory_DesignationId",
                schema: "HR",
                table: "Signatory",
                column: "DesignationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Signatory",
                schema: "HR");
        }
    }
}
