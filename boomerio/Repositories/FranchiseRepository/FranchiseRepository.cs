using boomerio.Data;
using boomerio.Models;
using Microsoft.EntityFrameworkCore;

namespace boomerio.Repositories.FranchiseRepository
{
    public class FranchiseRepository : IFranchiseRepository
    {
        private readonly AppDbContext _context;

        public FranchiseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<FranchiseModel?> GetById(int id)
        {
            return await _context
                .Franchises.Include(f => f.Characters)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<List<FranchiseModel>> GetAll()
        {
            return await _context.Franchises.Include(f => f.Characters).ToListAsync();
        }
    }
}
