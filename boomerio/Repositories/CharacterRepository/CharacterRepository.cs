using boomerio.Data;
using boomerio.Models;
using Microsoft.EntityFrameworkCore;

namespace boomerio.Repositories.CharacterRepository
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly AppDbContext _context;

        public CharacterRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CharacterModel?> GetById(int id)
        {
            return await _context
                .Characters.AsNoTracking().Include(c => c.Franchise).Include(c => c.Quotes)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<CharacterModel>> GetAll()
        {
            return await _context.Characters.AsNoTracking().Include(c => c.Franchise).ToListAsync();
        }

        public async Task<IEnumerable<CharacterModel>> GetByFranchiseId(int franchiseId)
        {
            return await _context
                .Characters.AsNoTracking().Include(c => c.Franchise)
                .Where(c => c.Franchise.Id == franchiseId)
                .ToListAsync();
        }
    }
}
