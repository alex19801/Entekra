using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Entekra.Data.Migrations
{
    public partial class AddedExpirated24HChecklist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Expirated24HChecklist",
                columns: table => new
                {
                    Expirated24HChecklistId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    ProjectExternalId = table.Column<int>(nullable: false),
                    ChecklistExternalId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expirated24HChecklist", x => x.Expirated24HChecklistId)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    ProjectId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectExternalId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Number = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectsId", x => x.ProjectId)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "Checklist",
                columns: table => new
                {
                    ChecklistId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChecklistExternalId = table.Column<int>(nullable: false),
                    ChecklistName = table.Column<string>(nullable: true),
                    ChecklistNumber = table.Column<string>(nullable: true),
                    Closed = table.Column<bool>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistId", x => x.ChecklistId)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_Checklist_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExtensionsDataList",
                columns: table => new
                {
                    ExtensionsDataListId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExtensionsDataListExternalId = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    CheckListId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtensionsDataListId", x => x.ExtensionsDataListId)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_ExtensionsDataList_Checklist_CheckListId",
                        column: x => x.CheckListId,
                        principalTable: "Checklist",
                        principalColumn: "ChecklistId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Issue",
                columns: table => new
                {
                    IssueId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IssueExternalId = table.Column<int>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    InspectionType = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    IssueNumber = table.Column<string>(nullable: true),
                    ProjectId = table.Column<int>(nullable: false),
                    ExtensionsDataListId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssuesId", x => x.IssueId)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_Issue_ExtensionsDataList_ExtensionsDataListId",
                        column: x => x.ExtensionsDataListId,
                        principalTable: "ExtensionsDataList",
                        principalColumn: "ExtensionsDataListId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Issue_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Checklist_ProjectId",
                table: "Checklist",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtensionsDataList_CheckListId",
                table: "ExtensionsDataList",
                column: "CheckListId");

            migrationBuilder.CreateIndex(
                name: "IX_Issue_ExtensionsDataListId",
                table: "Issue",
                column: "ExtensionsDataListId");

            migrationBuilder.CreateIndex(
                name: "IX_Issue_ProjectId",
                table: "Issue",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "UK_Projects_ProjectId",
                table: "Project",
                column: "ProjectId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expirated24HChecklist");

            migrationBuilder.DropTable(
                name: "Issue");

            migrationBuilder.DropTable(
                name: "ExtensionsDataList");

            migrationBuilder.DropTable(
                name: "Checklist");

            migrationBuilder.DropTable(
                name: "Project");
        }
    }
}
