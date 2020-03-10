using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using App.Data.Model;

namespace App.Data.Model.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .ToTable("USER")
                .HasKey(u => u.Id);
            builder.Property(u => u.Email)
                .IsRequired();
        }
    }
}