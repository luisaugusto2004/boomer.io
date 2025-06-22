using System.Text.Json.Serialization;

namespace boomerio.Models
{
    public class CharacterModel
    {
        public int Id { get; init; }
        public string Name { get; set; } = string.Empty;
        public FranchiseModel Franchise { get; set; } = new FranchiseModel();
        [JsonIgnore]
        public ICollection<QuoteModel> Quotes { get; set; } = new List<QuoteModel>();
    }
}