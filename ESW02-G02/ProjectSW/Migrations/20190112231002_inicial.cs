using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectSW.Migrations
{
    public partial class inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExitForm",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AnimalId = table.Column<string>(nullable: true),
                    ReportId = table.Column<string>(nullable: true),
                    AdopterName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Motive = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExitForm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExitForm_Animal_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExitForm_AnimalId",
                table: "ExitForm",
                column: "AnimalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExitForm");
        }
    }
}
