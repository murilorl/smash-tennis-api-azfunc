using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

using App.Data.Model;
using Core.Configuration;

namespace App.Data
{

    public class AppDbContext : DbContext
    {
        // private readonly ConfigurationItems _configurationItems;
        public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name &&
                    level == LogLevel.Information)
                .AddConsole();
        });

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        /*         public AppDbContext(DbContextOptions<AppDbContext> options, IOptions<ConfigurationItems> configurationItems) : base(options)
                {
                    _configurationItems = configurationItems.Value;
                } */

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(loggerFactory);

            // optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Applies model configuration for all classes that implement IEntityTypeConfiguration<TEntity> in the current assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<BackhandStyle> BackhandStyles { get; set; }
        public DbSet<DominantHand> DominantHands { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
    }
}