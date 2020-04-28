using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using App.Data.Model.Users;

namespace App.Data.Model.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder
                .ToTable("ROLES")
                .HasKey(u => u.RoleId);

            // Foreign keys

            //Entity attributes
            builder.Property(u => u.RoleId)
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

            builder.Property(u => u.IsDeleted)
                .HasColumnName("DELETED")
                .HasColumnType("BIT")
                .HasDefaultValue(0)
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
                        RoleId = 1,
                        Name = "Administrador",
                    },
                    new Role
                    {
                        RoleId = 2,
                        Name = "Tenista"
                    }
                );
        }
    }
}