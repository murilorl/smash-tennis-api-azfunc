using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Model.Configuration
{
    public class BackhandStyleConfiguration : IEntityTypeConfiguration<BackhandStyle>
    {
        public void Configure(EntityTypeBuilder<BackhandStyle> builder)
        {
            builder
                .ToTable("BACKHAND_STYLES")
                .HasKey(bhs => bhs.Id);

            builder.Property(bhs => bhs.Id)
                .HasColumnName("ID");

            // Foreign keys
            builder
                .HasMany(bhs => bhs.Users)
                .WithOne(u => u.BackhandStyle)
                .HasConstraintName("FK_BACKHAND_STYLE_ID")
                .IsRequired();

            builder.Property(bhs => bhs.Name)
                .HasColumnName("NAME")
                .HasMaxLength(70)
                .IsRequired();

            // Seed data
            builder
                .HasData(
                    new BackhandStyle
                    {
                        Id = 1,
                        Name = "Uma mão"
                    },
                    new BackhandStyle
                    {
                        Id = 2,
                        Name = "Duas mãos"
                    }
                );
        }
    }
}