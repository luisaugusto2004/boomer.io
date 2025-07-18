﻿using System.Text.Json.Serialization;

namespace boomerio.Models
{
    public class FranchiseModel
    {
        public int Id { get; init; }
        public string Name { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
        // TODO: Implement characters in DTO to return characters from a franchise
        [JsonIgnore]
        public ICollection<CharacterModel> Characters { get; set; } = new List<CharacterModel>();
    }
}
