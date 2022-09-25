using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEndDotnetPlumsail.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppVersions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Hash = table.Column<string>(maxLength: 255, nullable: true),
                    File = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Json = table.Column<string>(nullable: true),
                    Keywords = table.Column<string>(nullable: true),
                    AppVersionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rations_AppVersions_AppVersionId",
                        column: x => x.AppVersionId,
                        principalTable: "AppVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rations_AppVersionId",
                table: "Rations",
                column: "AppVersionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rations");

            migrationBuilder.DropTable(
                name: "AppVersions");
        }
    }
}
