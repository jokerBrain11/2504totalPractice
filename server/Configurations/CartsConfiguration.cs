using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Models;

namespace server.Configurations;

public class CartsConfiguration : IEntityTypeConfiguration<Carts>
{
    public void Configure(EntityTypeBuilder<Carts> builder)
    {
        builder.ToTable("Carts");

        builder.HasKey(e => e.CartId);

        builder.Property(e => e.CartId)
            .HasColumnName("CartId")
            .HasColumnType("int")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(e => e.UserId)
            .HasColumnName("UserId")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(e => e.ProductId)
            .HasColumnName("ProductId")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(e => e.Quantity)
            .HasColumnName("Quantity")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("datetime")
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");
    }
}
