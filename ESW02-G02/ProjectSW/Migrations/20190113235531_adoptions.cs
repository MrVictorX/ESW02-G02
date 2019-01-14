using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace ProjectSW.Migrations
{
    public partial class adoptions : Migration
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
                   EntryDate = table.Column<DateTime>(nullable: false),
                   EmployeeId = table.Column<string>(nullable: true)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_AnimalMonitoringReport", x => x.Id);
               });

            migrationBuilder.CreateTable(
                name: "Breed",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Breed", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Animal",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Size = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    BreedId = table.Column<string>(nullable: true),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    Foto = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Animal_Breed_BreedId",
                        column: x => x.BreedId,
                        principalTable: "Breed",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AnimalId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    File = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachment_Animal_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                    table.ForeignKey(
                        name: "FK_ExitForm_AnimalMonitoringReport_ReportId",
                        column: x => x.ReportId,
                        principalTable: "AnimalMonitoringReport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
               name: "IX_Animal_BreedId",
               table: "Animal",
               column: "BreedId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_AnimalId",
                table: "Attachment",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
               name: "IX_ExitForm_AnimalId",
               table: "ExitForm",
               column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_ExitForm_ReportId",
                table: "ExitForm",
                column: "ReportId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DropTable(
                name: "ExitForm");

            migrationBuilder.DropTable(
                name: "Animal");

            migrationBuilder.DropTable(
                name: "AnimalMonitoringReport");

            migrationBuilder.DropTable(
               name: "Breed");
        }
    }
}
