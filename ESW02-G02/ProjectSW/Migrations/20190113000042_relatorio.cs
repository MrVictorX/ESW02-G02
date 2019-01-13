using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectSW.Migrations
{
    public partial class relatorio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ReportId",
                table: "ExitForm",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExitForm_ReportId",
                table: "ExitForm",
                column: "ReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExitForm_AnimalMonitoringReport_ReportId",
                table: "ExitForm",
                column: "ReportId",
                principalTable: "AnimalMonitoringReport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExitForm_AnimalMonitoringReport_ReportId",
                table: "ExitForm");

            migrationBuilder.DropIndex(
                name: "IX_ExitForm_ReportId",
                table: "ExitForm");

            migrationBuilder.AlterColumn<string>(
                name: "ReportId",
                table: "ExitForm",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
