using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Monitoring.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MonitoringClient",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitoringClient", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MonitoringConfiguration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Configuration = table.Column<string>(type: "TEXT", nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitoringConfiguration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MonitoringLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Level = table.Column<string>(type: "TEXT", nullable: true),
                    Logger = table.Column<string>(type: "TEXT", nullable: true),
                    Message = table.Column<string>(type: "TEXT", nullable: true),
                    Exception = table.Column<string>(type: "TEXT", nullable: true),
                    Stacktrace = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitoringLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MonitoringReport",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ConfigId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClientId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TaskId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TaskType = table.Column<string>(type: "TEXT", nullable: true),
                    Result = table.Column<string>(type: "TEXT", nullable: true),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    MonitoringConfigurationId = table.Column<Guid>(type: "TEXT", nullable: true),
                    MonitoringClientId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitoringReport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonitoringReport_MonitoringClient_MonitoringClientId",
                        column: x => x.MonitoringClientId,
                        principalTable: "MonitoringClient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MonitoringReport_MonitoringConfiguration_MonitoringConfigurationId",
                        column: x => x.MonitoringConfigurationId,
                        principalTable: "MonitoringConfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MonitoringReport_MonitoringClientId",
                table: "MonitoringReport",
                column: "MonitoringClientId");

            migrationBuilder.CreateIndex(
                name: "IX_MonitoringReport_MonitoringConfigurationId",
                table: "MonitoringReport",
                column: "MonitoringConfigurationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonitoringLog");

            migrationBuilder.DropTable(
                name: "MonitoringReport");

            migrationBuilder.DropTable(
                name: "MonitoringClient");

            migrationBuilder.DropTable(
                name: "MonitoringConfiguration");
        }
    }
}
