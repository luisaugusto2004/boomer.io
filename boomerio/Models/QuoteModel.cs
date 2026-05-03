namespace boomerio.Models
{
    public class QuoteModel
    {
        public int Id { get; init; }
        public string QuoteText { get; set; } = string.Empty;
        public int CharacterId { get; set; }
        public CharacterModel Character { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public QuoteModel()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
