using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceHealthReader.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPostsBackIn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceHealthIssuePost",
                columns: table => new
                {
                    CreatedDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    PostType = table.Column<int>(type: "int", nullable: true),
                    AdditionalData = table.Column<string>(type: "TEXT", nullable: true),
                    ODataType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceHealthIssueId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceHealthIssuePost", x => x.CreatedDateTime);
                    table.ForeignKey(
                        name: "FK_ServiceHealthIssuePost_ServiceHealthIssues_ServiceHealthIssueId",
                        column: x => x.ServiceHealthIssueId,
                        principalTable: "ServiceHealthIssues",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceHealthIssuePost_ServiceHealthIssueId",
                table: "ServiceHealthIssuePost",
                column: "ServiceHealthIssueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceHealthIssuePost");
        }
    }
}
