using boomerio.DTOs.FranchiseDTOs;
using boomerio.Services.FranchiseService;
using Microsoft.Extensions.Caching.Memory;

namespace boomerio.Services.Cache.FranchisesCache
{
    public class FranchiseCacheService : IFranchiseService
    {
        private readonly IFranchiseService _inner;
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheEntryOptions _cacheEntryOptions;

        public FranchiseCacheService(IFranchiseService inner, IMemoryCache cache)
        {
            _inner = inner;
            _cache = cache;
            _cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };
        }

        public async Task<List<FranchiseDto>> GetAllAsync()
        {
            string cacheKey = "quotes_all";

            if (_cache.TryGetValue(cacheKey, out List<FranchiseDto>? cachedFranchises) && cachedFranchises is not null)
            {
                return cachedFranchises;
            }
            var franchises = await _inner.GetAllAsync();
            _cache.Set(cacheKey, franchises, _cacheEntryOptions);
            return franchises;
        }

        public Task<FranchiseDto?> GetById(int id) => _inner.GetById(id);
    }
}
