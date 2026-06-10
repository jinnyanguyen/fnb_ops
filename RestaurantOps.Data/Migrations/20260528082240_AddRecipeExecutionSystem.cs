using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantOps.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRecipeExecutionSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecipeExecutions",
                columns: table => new
                {
                    RecipeExecutionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeExecutions", x => x.RecipeExecutionId);
                    table.ForeignKey(
                        name: "FK_RecipeExecutions_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeExecutions_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeExecutions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RecipeSteps",
                columns: table => new
                {
                    RecipeStepId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Instruction = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StepOrder = table.Column<int>(type: "int", nullable: false),
                    RecipeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeSteps", x => x.RecipeStepId);
                    table.ForeignKey(
                        name: "FK_RecipeSteps_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RecipeExecutionSteps",
                columns: table => new
                {
                    RecipeExecutionStepId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RecipeExecutionId = table.Column<int>(type: "int", nullable: false),
                    RecipeStepId = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeExecutionSteps", x => x.RecipeExecutionStepId);
                    table.ForeignKey(
                        name: "FK_RecipeExecutionSteps_RecipeExecutions_RecipeExecutionId",
                        column: x => x.RecipeExecutionId,
                        principalTable: "RecipeExecutions",
                        principalColumn: "RecipeExecutionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeExecutionSteps_RecipeSteps_RecipeStepId",
                        column: x => x.RecipeStepId,
                        principalTable: "RecipeSteps",
                        principalColumn: "RecipeStepId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeExecutions_BranchId",
                table: "RecipeExecutions",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeExecutions_RecipeId",
                table: "RecipeExecutions",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeExecutions_UserId",
                table: "RecipeExecutions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeExecutionSteps_RecipeExecutionId",
                table: "RecipeExecutionSteps",
                column: "RecipeExecutionId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeExecutionSteps_RecipeStepId",
                table: "RecipeExecutionSteps",
                column: "RecipeStepId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeSteps_RecipeId",
                table: "RecipeSteps",
                column: "RecipeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeExecutionSteps");

            migrationBuilder.DropTable(
                name: "RecipeExecutions");

            migrationBuilder.DropTable(
                name: "RecipeSteps");
        }
    }
}
