namespace boomer_shooter_api.DTOs.CharacterDTOs
{
    public class CharacterDto
    {
        public int Id { get; set; }
        public string Franchise { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
