using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Model.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder
                .ToTable("ROLES")
                .HasKey(u => u.Id);

            // Foreign keys

            //Entity attributes
            builder.Property(u => u.Id)
                .HasColumnName("ID");

            builder.Property(u => u.Created)
                .HasColumnName("CREATED")
                .HasColumnType("DATETIME2")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.Property(u => u.Updated)
                .HasColumnName("UPDATED")
                .HasColumnType("DATETIME2")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.Property(u => u.Active)
                .HasColumnName("ACTIVE")
                .HasColumnType("BIT")
                .HasDefaultValue(1)
                .IsRequired();

            builder.Property(u => u.Name)
                .HasColumnName("NAME")
                .HasMaxLength(70)
                .IsRequired();

            // Seed data
            builder
                .HasData(
                    new Role
                    {
                        Id = Guid.NewGuid(),
                        Name = "Administrador",
                    },
                    new Role
                    {
                        Id = Guid.NewGuid(),
                        Name = "Tenista"
                    }
                );
        }
    }
}