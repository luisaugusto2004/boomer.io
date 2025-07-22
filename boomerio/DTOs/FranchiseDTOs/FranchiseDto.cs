using boomerio.DTOs.CharacterDTOs;
using boomerio.Models;

namespace boomerio.DTOs.FranchiseDTOs
{
    public class FranchiseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
        public List<CharacterDtoWithoutFranchise> Characters { get; set; } = new();
    }
}
