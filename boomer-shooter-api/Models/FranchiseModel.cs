using System.Text.Json.Serialization;

namespace boomer_shooter_api.Models
{
    public class FranchiseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<CharacterModel> Characters { get; set; }
    }
}
