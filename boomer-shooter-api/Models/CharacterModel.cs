using System.Text.Json.Serialization;

namespace boomer_shooter_api.Models
{
    public class CharacterModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public FranchiseModel Franchise { get; set; }
        [JsonIgnore]
        public ICollection<QuoteModel> Quotes { get; set; }
    }
}