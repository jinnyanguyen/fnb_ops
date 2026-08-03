using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantOps.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIntegrationMappings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExternalBranchMappings",
                columns: table => new
                {
                    ExternalBranchMappingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SourceSystem = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExternalStoreId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalBranchMappings", x => x.ExternalBranchMappingId);
                    table.ForeignKey(
                        name: "FK_ExternalBranchMappings_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ExternalRecipeMappings",
                columns: table => new
                {
                    ExternalRecipeMappingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SourceSystem = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExternalItemId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalRecipeMappings", x => x.ExternalRecipeMappingId);
                    table.ForeignKey(
                        name: "FK_ExternalRecipeMappings_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ImportedSaleRecords",
                columns: table => new
                {
                    ImportedSaleRecordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SourceSystem = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExternalSaleId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImportedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsSuccessful = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Message = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportedSaleRecords", x => x.ImportedSaleRecordId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalBranchMappings_BranchId",
                table: "ExternalBranchMappings",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalBranchMappings_SourceSystem_ExternalStoreId",
                table: "ExternalBranchMappings",
                columns: new[] { "SourceSystem", "ExternalStoreId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExternalRecipeMappings_RecipeId",
                table: "ExternalRecipeMappings",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalRecipeMappings_SourceSystem_ExternalItemId",
                table: "ExternalRecipeMappings",
                columns: new[] { "SourceSystem", "ExternalItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImportedSaleRecords_SourceSystem_ExternalSaleId",
                table: "ImportedSaleRecords",
                columns: new[] { "SourceSystem", "ExternalSaleId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalBranchMappings");

            migrationBuilder.DropTable(
                name: "ExternalRecipeMappings");

            migrationBuilder.DropTable(
                name: "ImportedSaleRecords");
        }
    }
}
