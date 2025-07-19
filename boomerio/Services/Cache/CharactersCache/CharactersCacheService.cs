using boomerio.DTOs.CharacterDTOs;
using boomerio.Services.CharacterService;
using Microsoft.Extensions.Caching.Memory;

namespace boomerio.Services.Cache.CharactersCache
{
    public class CharactersCacheService : ICharacterService
    {
        private readonly ICharacterService _inner;
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheEntryOptions _cacheOptions;

        public CharactersCacheService(ICharacterService inner, IMemoryCache cache)
        {
            _inner = inner;
            _cache = cache;
            _cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };
        }

        public async Task<List<CharacterDto>> GetAllAsync()
        {
            string cacheKey = "characters_all";

            if(_cache.TryGetValue(cacheKey, out List<CharacterDto>? cachedCharacters) && cachedCharacters is not null)
            {
                return cachedCharacters;
            }

            var characters = await _inner.GetAllAsync();
            _cache.Set(cacheKey, characters, _cacheOptions);
            return characters;
        }

        public Task<List<CharacterDto>> GetByFranchiseId(int idFranchise) => _inner.GetByFranchiseId(idFranchise);

        public Task<CharacterDto?> GetById(int id) => _inner.GetById(id);
    }
}
