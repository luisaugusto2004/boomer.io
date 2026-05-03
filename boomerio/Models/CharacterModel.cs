using System.Text.Json.Serialization;

namespace boomerio.Models
{
    public class CharacterModel
    {
        public int Id { get; init; }
        public string Name { get; set; } = string.Empty;
        public int FranchiseId { get; set; }
        public FranchiseModel Franchise { get; set; } = null!;

        [JsonIgnore]
        public ICollection<QuoteModel> Quotes { get; set; } = new List<QuoteModel>();
    }
}
