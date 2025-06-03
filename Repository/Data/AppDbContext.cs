using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<WelcomeBanner> WelcomeBanner { get; set; }
        public DbSet<SpecialGameBanner> SpecialGameBanner { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<GameCategory> GameCategories { get; set; }
        public DbSet<GameDiscount> GameDiscounts { get; set; }
        public DbSet<GameImage> GameImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Domain.Models.Console> Consoles { get; set; }
        public DbSet<ConsoleCategory> ConsoleCategories { get; set; }
        public DbSet<ConsoleImage> ConsoleImages { get; set; }
        public DbSet<ConsoleDiscount> ConsoleDiscounts { get; set; }
    }
}
