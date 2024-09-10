using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Models;

namespace server.Configurations;

public class ProductsConfiguration : IEntityTypeConfiguration<Products>
{
    public void Configure(EntityTypeBuilder<Products> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(e => e.ProductId);

        builder.Property(e => e.ProductId)
            .HasColumnName("ProductId")
            .HasColumnType("int")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(e => e.ProductName)
            .HasColumnName("ProductName")
            .HasColumnType("nvarchar(100)")
            .IsRequired();

        builder.Property(e => e.Description)
            .HasColumnName("ProductDescription")
            .HasColumnType("nvarchar(1000)")
            .IsRequired();

        builder.Property(e => e.Price)
            .HasColumnName("ProductPrice")
            .HasColumnType("decimal(18, 2)")
            .IsRequired();

        builder.Property(e => e.StockQuantity)
            .HasColumnName("StockQuantity")
            .HasColumnType("int")
            .IsRequired();

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
