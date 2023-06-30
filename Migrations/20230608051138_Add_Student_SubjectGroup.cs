using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCaThi.Migrations
{
    /// <inheritdoc />
    public partial class Add_Student_SubjectGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubjectGroup",
                table: "Students",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubjectGroup",
                table: "Students");
        }
    }
}
