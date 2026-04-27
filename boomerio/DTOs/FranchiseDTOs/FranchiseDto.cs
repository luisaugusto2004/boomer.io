using boomerio.DTOs.CharacterDTOs;
using System.Text.Json.Serialization;

namespace boomerio.DTOs.FranchiseDTOs
{
    public class FranchiseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
        [JsonIgnore]
        public IEnumerable<CharacterDto>? Characters { get; set; }
    }
}
