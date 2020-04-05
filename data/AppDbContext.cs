using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using App.Data.Model;
using Core.Configuration;

namespace App.Data
{

    public class AppDbContext : DbContext
    {
        private readonly ConfigurationItems _configurationItems;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public AppDbContext(DbContextOptions<AppDbContext> options, IOptions<ConfigurationItems> configurationItems) : base(options)
        {
            _configurationItems = configurationItems.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var sql = _configurationItems.SqlConnectionString;
            //Inject IOptions<ConfigurationItems> configurationItems in the constructor
            //get ConnectionString thru something like configurationItems.GetSection("ConfigurationItems")["SqlConnectionString"])
            optionsBuilder.UseSqlServer(sql);
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