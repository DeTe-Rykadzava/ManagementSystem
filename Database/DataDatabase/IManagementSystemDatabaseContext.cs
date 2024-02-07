using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;

namespace Database.Data;

public interface IManagementSystemDatabaseContext
{
    public DatabaseFacade Database { get; }
    
    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderComposition> OrderCompositions { get; set; }

    public DbSet<OrderPaymentType> OrderPaymentTypes { get; set; }

    public DbSet<OrderStatus> OrderStatuses { get; set; }

    public DbSet<OrderTypeSale> OrderTypeSales { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<ProductCategory> ProductCategories { get; set; }

    public DbSet<ProductPhoto> ProductPhotos { get; set; }

    public DbSet<ProductWarehouse> ProductWarehouses { get; set; }

    public DbSet<Report> Reports { get; set; }

    public DbSet<ReportType> ReportTypes { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<UserInfo> UserInfos { get; set; }

    public DbSet<Warehouse> Warehouses { get; set; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
}