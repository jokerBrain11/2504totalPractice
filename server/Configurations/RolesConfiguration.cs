using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Models;

namespace server.Configurations;

public class RolesConfiguration : IEntityTypeConfiguration<Roles>
{
    public void Configure(EntityTypeBuilder<Roles> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(e => e.RoleId);

        builder.Property(e => e.RoleId)
            .HasColumnName("RoleId")
            .HasColumnType("int")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(e => e.RoleName)
            .HasColumnName("RoleName")
            .HasColumnType("nvarchar(50)")
            .IsRequired();
    }
}
