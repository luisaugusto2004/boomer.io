namespace boomer_shooter_api.Models
{
    public class QuoteModel
    {
        public int Id { get; set; }
        public string QuoteText { get; set; }
        public CharacterModel Character { get; set; }
    }
}