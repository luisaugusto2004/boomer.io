namespace boomerio.Models
{
    public class QuoteModel
    {
        public int Id { get; init; }
        public string QuoteText { get; set; } = string.Empty;
        public CharacterModel Character { get; set; } = new CharacterModel();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public QuoteModel()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
