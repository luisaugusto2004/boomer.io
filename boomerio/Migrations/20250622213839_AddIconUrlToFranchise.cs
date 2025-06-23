using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace boomerio.Migrations
{
    /// <inheritdoc />
    public partial class AddIconUrlToFranchise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IconUrl",
                table: "Franchises",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconUrl",
                table: "Franchises");
        }
    }
}
