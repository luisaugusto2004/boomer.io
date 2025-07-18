using boomerio.DTOs.QuoteDTOs;
using boomerio.Services.QuoteService;
using Microsoft.Extensions.Caching.Memory;

namespace boomerio.Services.Cache.QuotesCache
{
    public class QuoteCacheService : IQuoteService
    {
        private readonly IQuoteService _inner;
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheEntryOptions _cacheEntryOptions;

        public QuoteCacheService(IQuoteService inner, IMemoryCache cache)
        {
            _inner = inner;
            _cache = cache;
            _cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
            };
        }

        public async Task<List<QuoteDto>> GetAll()
        {
            string cacheKey = "quote_all";
            if (_cache.TryGetValue(cacheKey, out List<QuoteDto>? cachedQuotes) && cachedQuotes is not null) {
                return cachedQuotes;
            }

            var quotes = await _inner.GetAll();
            _cache.Set(cacheKey, quotes, _cacheEntryOptions);
            return quotes;
        }

        public async Task<List<QuoteDto>> GetByCharacterId(int idCharacter)
        {
            string cacheKey = $"quote_{idCharacter}";
            if(_cache.TryGetValue(cacheKey, out List<QuoteDto>? cachedQuotes) && cachedQuotes is not null)
            {
                return cachedQuotes;
            }
            var quotes = await _inner.GetByCharacterId(idCharacter);
            _cache.Set(cacheKey, quotes, _cacheEntryOptions);
            return quotes;
        }

        public Task<QuoteDto?> GetById(int id) => _inner.GetById(id);

        public Task<List<QuoteDto>> GetByQuery(string query) => _inner.GetByQuery(query);

        public Task<QuoteDto?> GetRandomQuote() => _inner.GetRandomQuote();
    }
}
