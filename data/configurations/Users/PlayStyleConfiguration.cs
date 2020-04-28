using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using App.Data.Model.Users;

namespace App.Data.Model.Configuration.Users
{
    public class PlayStyleConfiguration : IEntityTypeConfiguration<PlayStyle>
    {
        public void Configure(EntityTypeBuilder<PlayStyle> builder)
        {
            builder
                .ToTable("PLAY_STYLE")
                .HasKey(e => e.PlayStyleId);

            builder.Property(e => e.PlayStyleId)
                .HasColumnName("ID");

            builder.Property(e => e.Name)
                .HasColumnName("NAME")
                .HasMaxLength(70)
                .IsRequired();

            builder.Property(e => e.DominatHand)
                .HasColumnName("DOMINANT_HAND")
                .HasMaxLength(70)
                .IsRequired();

            // Seed data
            builder
                .HasData(
                    new PlayStyle
                    {
                        PlayStyleId = 1,
                        Name = "Destro; Backhand uma mão.",
                        DominatHand = "Direita"
                    },
                    new PlayStyle
                    {
                        PlayStyleId = 2,
                        Name = "Destro; Backhand duas mãos.",
                        DominatHand = "Direita"
                    },
                    new PlayStyle
                    {
                        PlayStyleId = 3,
                        Name = "Canhoto; Backhand uma mãos.",
                        DominatHand = "Esquerda"
                    },
                    new PlayStyle
                    {
                        PlayStyleId = 4,
                        Name = "Canhoto; Backhand duas mãos.",
                        DominatHand = "Esquerda"
                    }
                );
        }
    }
}