using System.Text.Json.Serialization;

namespace boomer_shooter_api.Models
{
    public class FranchiseModel
    {
        public int Id { get; init; }
        public string Name { get; set; } = string.Empty;
        [JsonIgnore]
        public ICollection<CharacterModel> Characters { get; set; } = new List<CharacterModel>();
    }
}
