using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantOps.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlignBranchTableSnapshot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // No physical schema change is required.
            // The deployed MySQL database already uses the singular "Branch" table.
            // This migration aligns EF Core's model snapshot with the live schema.
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No physical schema change is required.
        }
    }
}