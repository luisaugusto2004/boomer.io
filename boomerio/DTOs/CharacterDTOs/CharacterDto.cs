using System.Text.Json.Serialization;

namespace boomerio.DTOs.CharacterDTOs
{
    public class CharacterDto
    {
        public int Id { get; set; }
        public string Franchise { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        [JsonIgnore]
        public int FranchiseId { get; set; }
    }
}
