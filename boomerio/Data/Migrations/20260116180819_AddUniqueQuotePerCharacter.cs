using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace boomerio.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueQuotePerCharacter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Quotes_CharacterId",
                table: "Quotes");

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_CharacterId_QuoteText",
                table: "Quotes",
                columns: new[] { "CharacterId", "QuoteText" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Quotes_CharacterId_QuoteText",
                table: "Quotes");

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_CharacterId",
                table: "Quotes",
                column: "CharacterId");
        }
    }
}
