﻿using System.Security.Cryptography;
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

        public async Task<List<QuoteModel>> GetByCharacterId(int idCharacter)
        {
            return await _context
                .Quotes.Include(q => q.Character)
                .ThenInclude(q => q.Franchise)
                .Where(q => q.Character.Id == idCharacter)
                .ToListAsync();
        }

        public async Task<List<QuoteModel>> GetAllAsync()
        {
            return await _context
                .Quotes.Include(q => q.Character)
                .ThenInclude(q => q.Franchise)
                .ToListAsync();
        }

        public async Task<QuoteModel?> GetByIdAsync(int id)
        {
            return await _context
                .Quotes.Include(q => q.Character)
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
                .Quotes.Include(q => q.Character)
                .ThenInclude(c => c.Franchise)
                .OrderBy(q => q.Id)
                .Skip(index)
                .FirstAsync();

            return quote;
        }

        // TODO: Implement ToLower() function to query and quote value(not now and idk when)
        public async Task<List<QuoteModel>> GetByQueryAsync(string query)
        {
            return await _context
                .Quotes.Include(q => q.Character)
                .ThenInclude(c => c.Franchise)
                .Where(q => q.QuoteText.Contains(query))
                .ToListAsync();
        }
    }
}
