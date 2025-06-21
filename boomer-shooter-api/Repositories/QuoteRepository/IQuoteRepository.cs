using boomer_shooter_api.Models;

namespace boomer_shooter_api.Repositories.QuoteRepository
{
    public interface IQuoteRepository
    {
        Task<List<QuoteModel>> GetAllAsync();
        Task<QuoteModel?> GetByIdAsync(int id);
        Task<List<QuoteModel>> GetByCharacterId(int idCharacter);
        Task<QuoteModel> GetRandomQuote();
    }
}
