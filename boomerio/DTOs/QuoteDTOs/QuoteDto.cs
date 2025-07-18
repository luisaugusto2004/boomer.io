﻿using System.Text.Json.Serialization;

namespace boomerio.DTOs.QuoteDTOs
{
    public class QuoteDto
    {
        public int Id { get; set; }
        public string CreatedAt { get; set; } = string.Empty;
        public string UpdatedAt { get; set; } = string.Empty;
        public string Franchise { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
        public string Character { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;

        [JsonIgnore]
        public int CharacterId { get; set; }
    }
}
