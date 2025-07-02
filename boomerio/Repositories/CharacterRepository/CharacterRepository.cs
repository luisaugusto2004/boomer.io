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
                .Characters.Include(c => c.Franchise)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<CharacterModel>> GetAll()
        {
            return await _context.Characters.Include(c => c.Franchise).ToListAsync();
        }

        public Task<List<CharacterModel>> GetByFranchiseId(int franchiseId)
        {
            return _context
                .Characters.Include(c => c.Franchise)
                .Where(c => c.Franchise.Id == franchiseId)
                .ToListAsync();
        }
    }
}
