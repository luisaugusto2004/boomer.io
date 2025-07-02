using boomerio.Models;
using Microsoft.EntityFrameworkCore;

namespace boomerio.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<FranchiseModel> Franchises { get; set; }
        public DbSet<CharacterModel> Characters { get; set; }
        public DbSet<QuoteModel> Quotes { get; set; }
    }
}
