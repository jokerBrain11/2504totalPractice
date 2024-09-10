using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Models;

namespace server.Configurations;

public class OrderDetailsConfiguration : IEntityTypeConfiguration<OrderDetails>
{
    public void Configure(EntityTypeBuilder<OrderDetails> builder)
    {
        builder.ToTable("OrderDetails");

        builder.HasKey(e => e.OrderDetailId);

        builder.Property(e => e.OrderDetailId)
            .HasColumnName("OrderDetailId")
            .HasColumnType("int")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(e => e.OrderId)
            .HasColumnName("OrderId")
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

        builder.Property(e => e.UnitPrice)
            .HasColumnName("UnitPrice")
            .HasColumnType("decimal(18, 2)")
            .IsRequired();
    }
}
