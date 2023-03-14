using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceHealthReader.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTenantToMultipleAndFixDateTimeDefaultValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Tenants_TenantId",
                table: "Issues");

            migrationBuilder.DropIndex(
                name: "IX_Issues_TenantId",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Issues");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "ServerInfos",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.CreateTable(
                name: "TenantIssue",
                columns: table => new
                {
                    TenantId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IssueId = table.Column<int>(type: "int", nullable: false),
                    FirstSeen = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantIssue", x => new { x.IssueId, x.TenantId });
                    table.ForeignKey(
                        name: "FK_TenantIssue_Issues_IssueId",
                        column: x => x.IssueId,
                        principalTable: "Issues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TenantIssue_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TenantIssue_TenantId",
                table: "TenantIssue",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TenantIssue");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "ServerInfos");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "Issues",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Issues_TenantId",
                table: "Issues",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Tenants_TenantId",
                table: "Issues",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId");
        }
    }
}
