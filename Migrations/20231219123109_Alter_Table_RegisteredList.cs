using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCaThi.Migrations
{
    /// <inheritdoc />
    public partial class Alter_Table_RegisteredList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListRegisteds");

            migrationBuilder.CreateTable(
                name: "RegisteredLists",
                columns: table => new
                {
                    RegisteredListID = table.Column<Guid>(type: "TEXT", nullable: false),
                    ExamTimeID = table.Column<Guid>(type: "TEXT", nullable: false),
                    StudentID = table.Column<Guid>(type: "TEXT", nullable: false),
                    SubjectID = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisteredLists", x => x.RegisteredListID);
                    table.ForeignKey(
                        name: "FK_RegisteredLists_ExamTimes_ExamTimeID",
                        column: x => x.ExamTimeID,
                        principalTable: "ExamTimes",
                        principalColumn: "ExamTimeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegisteredLists_Students_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Students",
                        principalColumn: "StudentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegisteredLists_Subjects_SubjectID",
                        column: x => x.SubjectID,
                        principalTable: "Subjects",
                        principalColumn: "SubjectID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredLists_ExamTimeID",
                table: "RegisteredLists",
                column: "ExamTimeID");

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredLists_StudentID",
                table: "RegisteredLists",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredLists_SubjectID",
                table: "RegisteredLists",
                column: "SubjectID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegisteredLists");

            migrationBuilder.CreateTable(
                name: "ListRegisteds",
                columns: table => new
                {
                    ListRegistedID = table.Column<Guid>(type: "TEXT", nullable: false),
                    ExamTimeID = table.Column<Guid>(type: "TEXT", nullable: false),
                    StudentID = table.Column<Guid>(type: "TEXT", nullable: false),
                    SubjectID = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListRegisteds", x => x.ListRegistedID);
                    table.ForeignKey(
                        name: "FK_ListRegisteds_ExamTimes_ExamTimeID",
                        column: x => x.ExamTimeID,
                        principalTable: "ExamTimes",
                        principalColumn: "ExamTimeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListRegisteds_Students_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Students",
                        principalColumn: "StudentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListRegisteds_Subjects_SubjectID",
                        column: x => x.SubjectID,
                        principalTable: "Subjects",
                        principalColumn: "SubjectID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListRegisteds_ExamTimeID",
                table: "ListRegisteds",
                column: "ExamTimeID");

            migrationBuilder.CreateIndex(
                name: "IX_ListRegisteds_StudentID",
                table: "ListRegisteds",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_ListRegisteds_SubjectID",
                table: "ListRegisteds",
                column: "SubjectID");
        }
    }
}
