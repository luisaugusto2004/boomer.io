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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuoteModel>()
                .HasIndex(q => new { q.CharacterId, q.QuoteText })
                .IsUnique();
            modelBuilder.Entity<CharacterModel>()
                .HasOne(c => c.Franchise)
                .WithMany(f => f.Characters)
                .HasForeignKey(c => c.FranchiseId)
                .IsRequired();

            modelBuilder.Entity<QuoteModel>()
                .HasOne(q => q.Character)
                .WithMany(c => c.Quotes)
                .HasForeignKey(q => q.CharacterId)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
