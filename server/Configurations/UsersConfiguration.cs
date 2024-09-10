using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Models;

namespace server.Configurations;

public class UsersConfiguration : IEntityTypeConfiguration<Users>
{
    public void Configure(EntityTypeBuilder<Users> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(e => e.UserId);

        builder.Property(e => e.UserId)
            .HasColumnName("UserId")
            .HasColumnType("int")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Username)
            .HasColumnName("UserName")
            .HasColumnType("nvarchar(50)")
            .IsRequired();

        builder.Property(e => e.PasswordHash)
            .HasColumnName("Password")
            .HasColumnType("nvarchar(255)")
            .IsRequired();

        builder.Property(e => e.Email)
            .HasColumnName("Email")
            .HasColumnType("nvarchar(100)")
            .IsRequired();

        builder.Property(e => e.IsEmailVerified)
            .HasColumnName("IsEmailVerified")
            .HasColumnType("bit")
            .IsRequired()
            .HasDefaultValue(false);
        
        builder.Property(e => e.EmailVerificationToken)
            .HasColumnName("EmailVerificationToken")
            .HasColumnType("nvarchar(255)");
        
        builder.Property(e => e.FirstName)
            .HasColumnName("FirstName")
            .HasColumnType("nvarchar(50)");
        
        builder.Property(e => e.LastName)
            .HasColumnName("LastName")
            .HasColumnType("nvarchar(50)");
        
        builder.Property(e => e.Phone)
            .HasColumnName("Phone")
            .HasColumnType("nvarchar(20)");
        
        builder.Property(e => e.Address)
            .HasColumnName("Address")
            .HasColumnType("nvarchar(255)");
        
        builder.Property(e => e.EmailVerificationTokenExpires)
            .HasColumnName("EmailVerificationTokenExpires")
            .HasColumnType("datetime");

        builder.Property(e => e.PasswordResetToken)
            .HasColumnName("PasswordResetToken")
            .HasColumnType("nvarchar(255)");

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