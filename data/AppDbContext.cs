using System;
using Microsoft.EntityFrameworkCore;

using App.Data.Model;

namespace App.Data
{

    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }

    }
}