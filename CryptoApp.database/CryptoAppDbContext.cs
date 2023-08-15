using Microsoft.EntityFrameworkCore;

namespace CryptoApp.database
{
    public class CryptoAppDbContext : DbContext
    {
        public DbSet<CryptoAsset> CryptoAssets { get; set; }

        public CryptoAppDbContext(DbContextOptions options) : base(options) {
            Database.EnsureCreated();
        }
    }
}