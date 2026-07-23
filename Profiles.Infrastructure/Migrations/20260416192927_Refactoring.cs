using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnoClinic.Profiles.Api.Migrations
{
    /// <inheritdoc />
    public partial class Refactoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhotoPath",
                table: "Doctors",
                newName: "PhotoPathPath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhotoPathPath",
                table: "Doctors",
                newName: "PhotoPath");
        }
    }
}
