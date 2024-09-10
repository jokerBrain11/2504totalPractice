using Microsoft.EntityFrameworkCore;
using server.Configurations;

namespace server.Models;

public class StoresDBContext : DbContext
{
    public StoresDBContext(DbContextOptions<StoresDBContext> options) : base(options)
    {
    }

    public virtual DbSet<Users> Users { get; set; }
    public virtual DbSet<Products> Products { get; set; }
    public virtual DbSet<Carts> Carts { get; set; }
    public virtual DbSet<Orders> Orders { get; set; }
    public virtual DbSet<OrderDetails> OrderDetails { get; set; }
    public virtual DbSet<Roles> Roles { get; set; }
    public virtual DbSet<UsersRoles> UsersRoles { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UsersConfiguration());
        modelBuilder.ApplyConfiguration(new ProductsConfiguration());
        modelBuilder.ApplyConfiguration(new CartsConfiguration());
        modelBuilder.ApplyConfiguration(new OrdersConfiguration());
        modelBuilder.ApplyConfiguration(new OrderDetailsConfiguration());
        modelBuilder.ApplyConfiguration(new RolesConfiguration());
        modelBuilder.ApplyConfiguration(new UsersRolesConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}