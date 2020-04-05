using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Model.Configuration
{
    public class DominantHandConfiguration : IEntityTypeConfiguration<DominantHand>
    {
        public void Configure(EntityTypeBuilder<DominantHand> builder)
        {
            builder
                .ToTable("DOMINANT_HAND")
                .HasKey(dh => dh.Id);

            builder.Property(dh => dh.Id)
                .HasColumnName("ID");

            builder.Property(dh => dh.Name)
                .HasColumnName("NAME")
                .HasMaxLength(70)
                .IsRequired();

            builder.Property(dh => dh.AltName)
                .HasColumnName("ALT_NAME")
                .HasMaxLength(30)
                .IsRequired();

            // Seed data
            builder
                .HasData(
                    new DominantHand
                    {
                        Id = 1,
                        Name = "Direita",
                        AltName = "Destro"
                    },
                    new DominantHand
                    {
                        Id = 2,
                        Name = "Esquerda",
                        AltName = "Canhoto"
                    }
                );
        }
    }
}