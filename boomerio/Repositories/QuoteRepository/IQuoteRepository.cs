using boomerio.Models;

namespace boomerio.Repositories.QuoteRepository
{
    public interface IQuoteRepository
    {
        Task<List<QuoteModel>> GetAllAsync();
        Task<QuoteModel?> GetByIdAsync(int id);
        Task<List<QuoteModel>> GetByCharacterId(int idCharacter);
        Task<QuoteModel> GetRandomQuote();
        Task<List<QuoteModel>> GetByTermAsync(string query);
    }
}
