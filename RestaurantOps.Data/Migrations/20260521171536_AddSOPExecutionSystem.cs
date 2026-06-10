using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantOps.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSOPExecutionSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SOPExecutions",
                columns: table => new
                {
                    SOPExecutionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SOPTemplateId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ExecutedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SOPExecutions", x => x.SOPExecutionId);
                    table.ForeignKey(
                        name: "FK_SOPExecutions_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SOPExecutions_SOPTemplates_SOPTemplateId",
                        column: x => x.SOPTemplateId,
                        principalTable: "SOPTemplates",
                        principalColumn: "SOPTemplateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SOPExecutions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SOPExecutionItems",
                columns: table => new
                {
                    SOPExecutionItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SOPExecutionId = table.Column<int>(type: "int", nullable: false),
                    SOPItemId = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SOPExecutionItems", x => x.SOPExecutionItemId);
                    table.ForeignKey(
                        name: "FK_SOPExecutionItems_SOPExecutions_SOPExecutionId",
                        column: x => x.SOPExecutionId,
                        principalTable: "SOPExecutions",
                        principalColumn: "SOPExecutionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SOPExecutionItems_SOPItems_SOPItemId",
                        column: x => x.SOPItemId,
                        principalTable: "SOPItems",
                        principalColumn: "SOPItemId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SOPExecutionItems_SOPExecutionId",
                table: "SOPExecutionItems",
                column: "SOPExecutionId");

            migrationBuilder.CreateIndex(
                name: "IX_SOPExecutionItems_SOPItemId",
                table: "SOPExecutionItems",
                column: "SOPItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SOPExecutions_BranchId",
                table: "SOPExecutions",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_SOPExecutions_SOPTemplateId",
                table: "SOPExecutions",
                column: "SOPTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_SOPExecutions_UserId",
                table: "SOPExecutions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SOPExecutionItems");

            migrationBuilder.DropTable(
                name: "SOPExecutions");
        }
    }
}
