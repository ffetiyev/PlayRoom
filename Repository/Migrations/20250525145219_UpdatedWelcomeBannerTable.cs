using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedWelcomeBannerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "WelcomeBanner",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "WelcomeBanner",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "WelcomeBanner");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "WelcomeBanner",
                newName: "ID");
        }
    }
}
