using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectSW.Migrations
{
    public partial class amr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnimalMonitoringReport",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    EntryDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalMonitoringReport", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimalMonitoringReport");
        }
    }
}
