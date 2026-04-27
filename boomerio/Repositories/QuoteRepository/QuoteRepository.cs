using System.Security.Cryptography;
using boomerio.Data;
using boomerio.Models;
using Microsoft.EntityFrameworkCore;

namespace boomerio.Repositories.QuoteRepository
{
    // Implementação
    public class QuoteRepository : IQuoteRepository
    {
        private readonly AppDbContext _context;

        public QuoteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<QuoteModel>> GetByCharacterId(int idCharacter)
        {
            return await _context
                .Quotes.AsNoTracking().Include(q => q.Character)
                .ThenInclude(q => q.Franchise)
                .Where(q => q.CharacterId == idCharacter)
                .OrderBy(q => q.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<QuoteModel>> GetAllAsync()
        {
            return await _context
                .Quotes.AsNoTracking().Include(q => q.Character)
                .ThenInclude(q => q.Franchise)
                .ToListAsync();
        }

        public async Task<QuoteModel?> GetByIdAsync(int id)
        {
            return await _context
                .Quotes.AsNoTracking().Include(q => q.Character)
                .ThenInclude(q => q.Franchise)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<QuoteModel?> GetRandomQuote()
        {
            int count = await _context.Quotes.CountAsync();

            if (count == 0)
            {
                return null;
            }
            int index = RandomNumberGenerator.GetInt32(_context.Quotes.Count());

            var quote = await _context
                .Quotes.AsNoTracking().Include(q => q.Character)
                .ThenInclude(c => c.Franchise)
                .OrderBy(q => q.Id)
                .Skip(index)
                .FirstAsync();

            return quote;
        }

        // TODO: Implement ToLower() function to query and quote value(not now and idk when)
        public async Task<IEnumerable<QuoteModel>> GetByQueryAsync(string query)
        {
            return await _context
                .Quotes.AsNoTracking().Include(q => q.Character)
                .ThenInclude(c => c.Franchise)
                .Where(q => q.QuoteText.Contains(query))
                .ToListAsync();
        }

        public async Task<QuoteModel> Create(QuoteModel quote)
        {
            await _context.AddAsync(quote);
            await _context.SaveChangesAsync();
            return quote;
        }
    }
}
