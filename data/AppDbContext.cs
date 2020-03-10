using Microsoft.EntityFrameworkCore;
using System.Reflection;

using App.Data.Model;
using App.Data.Model.Configuration;

namespace App.Data
{

    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

/*         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Smash;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        } */

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*             modelBuilder.Entity<User>().HasKey(u => u.Id);
                        modelBuilder.Entity<User>()
                            .Property(u => u.Email)
                            .IsRequired(); */
            // modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<User> Users { get; set; }

    }
}