using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Models;

namespace server.Configurations;

public class UsersRolesConfiguration : IEntityTypeConfiguration<UsersRoles>
{
    public void Configure(EntityTypeBuilder<UsersRoles> builder)
    {
        builder.ToTable("UsersRoles");

        builder.HasKey(e => e.UserRoleId);

        builder.Property(e => e.UserRoleId)
            .HasColumnName("UserRoleId")
            .HasColumnType("int")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(e => e.UserId)
            .HasColumnName("UserId")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(e => e.RoleId)
            .HasColumnName("RoleId")
            .HasColumnType("int")
            .IsRequired();
    }
}
