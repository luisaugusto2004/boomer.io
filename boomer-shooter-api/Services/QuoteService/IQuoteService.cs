using boomer_shooter_api.DTOs.QuoteDTOs;
using boomer_shooter_api.Models;

namespace boomer_shooter_api.Services.QuoteService
{
    public interface IQuoteService
    {
        Task<List<QuoteDto>> GetAll();
        Task<QuoteDto?> GetById(int id);
        Task<List<QuoteDto>> GetByCharacterId(int id);
        Task<QuoteDto?> GetRandomQuote();
    }
}
