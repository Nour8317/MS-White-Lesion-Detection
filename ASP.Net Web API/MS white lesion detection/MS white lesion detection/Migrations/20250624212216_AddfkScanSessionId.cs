using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MS_white_lesion_detection.Migrations
{
    public partial class AddfkScanSessionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScanSessionId",
                table: "Scans",
                type: "int",
                nullable: false,
                defaultValue: 0); // Consider making it nullable if existing rows exist

            migrationBuilder.CreateIndex(
                name: "IX_Scans_ScanSessionId",
                table: "Scans",
                column: "ScanSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Scans_ScanSessions_ScanSessionId",
                table: "Scans",
                column: "ScanSessionId",
                principalTable: "ScanSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scans_ScanSessions_ScanSessionId",
                table: "Scans");

            migrationBuilder.DropIndex(
                name: "IX_Scans_ScanSessionId",
                table: "Scans");

            migrationBuilder.DropColumn(
                name: "ScanSessionId",
                table: "Scans");
        }
    }
}
