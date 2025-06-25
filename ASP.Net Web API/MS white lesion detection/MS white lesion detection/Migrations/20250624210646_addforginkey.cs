using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MS_white_lesion_detection.Migrations
{
    /// <inheritdoc />
    public partial class addforginkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "Scans");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SessionId",
                table: "Scans",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
