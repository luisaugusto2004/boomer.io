namespace boomer_shooter_api.DTOs.QuoteDTOs
{
    public class QuoteDto
    {
        public int Id { get; set; }
        public string Franchise { get; set; } = string.Empty;
        public string Character { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
