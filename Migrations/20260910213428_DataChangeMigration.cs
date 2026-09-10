using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiagnosisRepositoryApi.Migrations
{
    /// <inheritdoc />
    public partial class DataChangeMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ICD10",
                table: "Diagnoses",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "SourceSystem",
                table: "Diagnoses",
                newName: "Provider");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Diagnoses",
                newName: "ICD10");

            migrationBuilder.RenameColumn(
                name: "Provider",
                table: "Diagnoses",
                newName: "SourceSystem");
        }
    }
}
