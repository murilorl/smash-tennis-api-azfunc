using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Model.Configuration
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRoles>
    {
        public void Configure(EntityTypeBuilder<UserRoles> builder)
        {
            builder
                .ToTable("USER_ROLES")
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            // Foreign keys
        }
    }
}