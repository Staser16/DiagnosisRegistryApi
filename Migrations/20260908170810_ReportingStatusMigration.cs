using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiagnosisRepositoryApi.Migrations
{
    /// <inheritdoc />
    public partial class ReportingStatusMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReportingStatus",
                table: "Diagnoses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportingStatus",
                table: "Diagnoses");
        }
    }
}
