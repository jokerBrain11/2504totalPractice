using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Models;

namespace server.Configurations;

public class OrdersConfiguration : IEntityTypeConfiguration<Orders>
{
    public void Configure(EntityTypeBuilder<Orders> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(e => e.OrderId);

        builder.Property(e => e.OrderId)
            .HasColumnName("OrderId")
            .HasColumnType("int")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(e => e.UserId)
            .HasColumnName("UserId")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(e => e.TotalAmount)
            .HasColumnName("TotalAmount")
            .HasColumnType("decimal(18, 2)")
            .IsRequired();

        builder.Property(e => e.OrderStatus)
            .HasColumnName("OrderStatus")
            .HasColumnType("nvarchar(50)")
            .IsRequired()
            .HasDefaultValue("Pending");

        builder.Property(e => e.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("datetime")
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.Property(e => e.UpdatedAt)
            .HasColumnName("UpdatedAt")
            .HasColumnType("datetime")
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");
    }
}
