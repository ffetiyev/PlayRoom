using Domain.Models;
using Domain.Models.Accessory;
using Domain.Models.Console;
using Domain.Models.Game;
using Domain.Models.News;
using Domain.Models.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Repository.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
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
        public DbSet<Domain.Models.Console.Console> Consoles { get; set; }
        public DbSet<ConsoleCategory> ConsoleCategories { get; set; }
        public DbSet<ConsoleImage> ConsoleImages { get; set; }
        public DbSet<ConsoleDiscount> ConsoleDiscounts { get; set; }
        public DbSet<Accessory> Accessories { get; set; }
        public DbSet<AccessoryCategory> AccessoryCategories { get; set; }
        public DbSet<AccessoryDiscount> AccessoryDiscounts { get; set; }
        public DbSet<AccessoryImage> AccessoryImages { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<DeliveryPayment> DeliveyPayment { get; set; }
        public DbSet<Warranty> Warranty { get; set; }
        public DbSet<Privacy> Privacy { get; set; }
        public DbSet<HomeShortcut> HomeShortcuts { get; set; }
        public DbSet<Promocode> Promocodes { get; set; }

    }
}
