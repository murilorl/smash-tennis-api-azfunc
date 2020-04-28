using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using App.Data.Model.Users;

namespace App.Data.Model.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            // Table name and primary key
            builder
                .ToTable("USERS")
                .HasKey(u => u.Id);

            // Indexes
            builder
                 .HasIndex(p => p.Email)
                 .IsUnique();

            //Query fitlers; for things like 'soft delete'
            builder.HasQueryFilter(e => e.IsDeleted != true);

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

            builder.Property(u => u.IsDeleted)
                .HasColumnName("DELETED")
                .HasColumnType("BIT")
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(u => u.Email)
                .HasColumnName("EMAIL")
                .HasMaxLength(70)
                .IsRequired();

            builder.Property(u => u.FirstName)
                .HasColumnName("FIRST_NAME")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(u => u.MiddleName)
                .HasColumnName("MIDDLE_NAME")
                .HasMaxLength(50);

            builder.Property(u => u.LastName)
                .HasColumnName("LAST_NAME")
                .HasMaxLength(70)
                .IsRequired();

            builder.Property(u => u.ShortName)
                .HasColumnName("SHORT_NAME")
                .HasMaxLength(120)
                .IsRequired();

            builder.Property(u => u.Birthday)
                .HasColumnName("BIRTHDAY")
                .HasColumnType("DATE");

            builder.Property(u => u.Weight)
                .HasColumnName("WEIGHT")
                .HasColumnType("INT");

            builder.Property(u => u.Height)
                .HasColumnName("HEIGHT")
                .HasColumnType("INT");

            builder.Property(u => u.FacebookId)
                .HasColumnName("FACEBOOK_ID")
                .HasMaxLength(70);

            builder.Property(u => u.Password)
                .HasColumnName("PASSWORD")
                .HasMaxLength(200);

            builder.Property(p => p.LastLogin)
                .HasColumnName("LAST_LOGIN")
                .HasColumnType("DATETIME2");

            //Seed data
            /*
            builder
                .HasData(
                    new User
                    {
                        Email = "john.doe@gmail.com",
                        FirstName = "John",
                        LastName = "Doe",
                        ShortName = "john-doe",
                        DominantHandId = 1,
                        BackhandStyleId = 1,
                        Weight = 86,
                        Height = 178
                    },
                    new User
                    {
                        Email = "novak.djokovic@gmail.com",
                        FirstName = "Novak",
                        LastName = "Djokovic",
                        ShortName = "novak-djokovic",
                        DominantHandId = 1,
                        BackhandStyleId = 2,
                        Password = "12345678",
                        Weight = 77,
                        Height = 188
                    },
                    new User
                    {
                        Email = "rafael.nadal@gmail.com",
                        FirstName = "Rafael",
                        LastName = "Nadal",
                        ShortName = "rafael-nadal",
                        DominantHandId = 2,
                        BackhandStyleId = 2,
                        Password = "12345678",
                        Weight = 85,
                        Height = 185
                    },
                    new User
                    {
                        Email = "dominic.thiem@gmail.com",
                        FirstName = "Dominic",
                        LastName = "Thiem",
                        ShortName = "dominic-thiem",
                        DominantHandId = 1,
                        BackhandStyleId = 1,
                        Password = "12345678",
                        Weight = 79,
                        Height = 185
                    },
                    new User
                    {
                        Email = "roger.federer@gmail.com",
                        FirstName = "Roger",
                        LastName = "Federer",
                        ShortName = "roger-federer",
                        DominantHandId = 1,
                        BackhandStyleId = 1,
                        Password = "12345678",
                        Weight = 85,
                        Height = 185
                    },
                    new User
                    {
                        Email = "daniil.medvedev@gmail.com",
                        FirstName = "Daniil",
                        LastName = "Medvedev",
                        ShortName = "daniil-medvedev",
                        DominantHandId = 1,
                        BackhandStyleId = 2,
                        Password = "12345678",
                        Weight = 83,
                        Height = 198
                    },
                    new User
                    {
                        Email = "stefanos.tsitsipas@gmail.com",
                        FirstName = "Stefanos",
                        LastName = "Tsitsipas",
                        ShortName = "stefanos-tsitsipas",
                        DominantHandId = 1,
                        BackhandStyleId = 2,
                        Password = "12345678",
                        Weight = 89,
                        Height = 193
                    }
                );
                */
        }
    }
}