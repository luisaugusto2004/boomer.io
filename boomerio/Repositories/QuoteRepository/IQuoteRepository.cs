using boomerio.Models;

namespace boomerio.Repositories.QuoteRepository
{
    public interface IQuoteRepository
    {
        Task<IEnumerable<QuoteModel>> GetAllAsync();
        Task<QuoteModel?> GetByIdAsync(int id);
        Task<QuoteModel?> GetByIdForUpdateAsync(int id);
        Task<IEnumerable<QuoteModel>> GetByCharacterId(int idCharacter);
        Task<QuoteModel?> GetRandomQuote();
        Task<IEnumerable<QuoteModel>> GetByQueryAsync(string query);
        Task<QuoteModel> Create(QuoteModel quote);
        Task Update(QuoteModel quote);
        Task Delete(QuoteModel quote);
    }
}
