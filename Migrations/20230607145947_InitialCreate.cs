using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCaThi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    SubjectID = table.Column<Guid>(type: "TEXT", nullable: false),
                    SubjectCode = table.Column<string>(type: "TEXT", nullable: false),
                    SubjectName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.SubjectID);
                });

            migrationBuilder.CreateTable(
                name: "ExamTimes",
                columns: table => new
                {
                    ExamTimeID = table.Column<Guid>(type: "TEXT", nullable: false),
                    ExamTimeName = table.Column<string>(type: "TEXT", nullable: false),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FinishTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExamDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MaxValue = table.Column<int>(type: "INTEGER", nullable: false),
                    RegistedValue = table.Column<int>(type: "INTEGER", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: false),
                    IsFull = table.Column<bool>(type: "INTEGER", nullable: false),
                    SubjectID = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamTimes", x => x.ExamTimeID);
                    table.ForeignKey(
                        name: "FK_ExamTimes_Subjects_SubjectID",
                        column: x => x.SubjectID,
                        principalTable: "Subjects",
                        principalColumn: "SubjectID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentID = table.Column<Guid>(type: "TEXT", nullable: false),
                    StudentCode = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    FullName = table.Column<string>(type: "TEXT", nullable: false),
                    BirthDay = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SubjectGroup = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    SubjectID = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentID);
                    table.ForeignKey(
                        name: "FK_Students_Subjects_SubjectID",
                        column: x => x.SubjectID,
                        principalTable: "Subjects",
                        principalColumn: "SubjectID",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_ExamTimes_SubjectID",
                table: "ExamTimes",
                column: "SubjectID");

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

            migrationBuilder.CreateIndex(
                name: "IX_Students_SubjectID",
                table: "Students",
                column: "SubjectID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListRegisteds");

            migrationBuilder.DropTable(
                name: "ExamTimes");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Subjects");
        }
    }
}
