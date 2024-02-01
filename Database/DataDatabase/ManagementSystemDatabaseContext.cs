using System;
using System.Collections.Generic;
using Database.Core;
using Microsoft.EntityFrameworkCore;

namespace Database.Data;

public sealed partial class ManagementSystemDatabaseContext : DbContext
{
    internal ManagementSystemDatabaseContext(DbContextOptions<ManagementSystemDatabaseContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    // private ManagementSystemDatabaseContext? _context;
    //
    // public ManagementSystemDatabaseContext Context 
    // {
    //     get
    //     {
    //         if(_context == null)
    //             _context = DatabaseSettings.CreateDbContext();
    //         return _context;
    //     }
    // }
    //
    // // public bool EnsureCreated()
    // // {
    // //     return Database.EnsureCreated();
    // // }

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

    // protected override async void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // => optionsBuilder.UseNpgsql(await DatabaseSettings.GetConnectionString());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_pk");

            entity.ToTable("order");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BuyerEmail).HasColumnName("buyer_email");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.CreateDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("create_date");
            entity.Property(e => e.PaymentTypeId).HasColumnName("payment_type_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.StatusUpdateDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("status_update_date");
            entity.Property(e => e.TypeSaleId).HasColumnName("type_sale_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.PaymentType).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_payment_type_fk");

            entity.HasOne(d => d.Status).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("order_status_fk");

            entity.HasOne(d => d.TypeSale).WithMany(p => p.Orders)
                .HasForeignKey(d => d.TypeSaleId)
                .HasConstraintName("order_type_sale_fk");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("order_user_fk");
        });

        modelBuilder.Entity<OrderComposition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_composition_pk");

            entity.ToTable("order_composition");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductCount).HasColumnName("product_count");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderCompositions)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("order_composition_order_fk");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderCompositions)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("order_composition_product_fk");
        });

        modelBuilder.Entity<OrderPaymentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_payment_type_pk");

            entity.ToTable("order_payment_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Type).HasColumnName("type");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_status_pk");

            entity.ToTable("order_status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.StatusName).HasColumnName("status_name");
        });

        modelBuilder.Entity<OrderTypeSale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_type_sale_pk");

            entity.ToTable("order_type_sale");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TypeName).HasColumnName("type_name");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_pk");

            entity.ToTable("product");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Title).HasColumnName("title");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("product_category_fk");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_category_pk");

            entity.ToTable("product_category");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryName).HasColumnName("category_name");
        });

        modelBuilder.Entity<ProductPhoto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_photo_pk");

            entity.ToTable("product_photo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductPhotos)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("product_photo_fk");
        });

        modelBuilder.Entity<ProductWarehouse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_warehouse_pk");

            entity.ToTable("product_warehouse");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.CountReserved).HasColumnName("count_reserved");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductWarehouses)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("product_warehouse_product_fk");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.ProductWarehouses)
                .HasForeignKey(d => d.WarehouseId)
                .HasConstraintName("product_warehouse_warehouse_fk");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("report_pk");

            entity.ToTable("report");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("create_date");
            entity.Property(e => e.Info).HasColumnName("info");
            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Type).WithMany(p => p.Reports)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("report_type_fk");

            entity.HasOne(d => d.User).WithMany(p => p.Reports)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("report_user_fk");
        });

        modelBuilder.Entity<ReportType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("report_type_pk");

            entity.ToTable("report_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pk");

            entity.ToTable("role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RoleName).HasColumnName("role_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("newtable_pk");

            entity.ToTable("user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.HashPassword).HasColumnName("hash_password");
            entity.Property(e => e.Login).HasColumnName("login");
            entity.Property(e => e.UserInfoId).HasColumnName("user_info_id");

            entity.HasOne(d => d.UserInfo).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserInfoId)
                .HasConstraintName("user_fk");
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_info_pk");

            entity.ToTable("user_info");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FirstName).HasColumnName("first_name");
            entity.Property(e => e.LastName).HasColumnName("last_name");
            entity.Property(e => e.Patronymic).HasColumnName("patronymic");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Role).WithMany(p => p.UserInfos)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("user_info_role_fk");
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("warehouse_pk");

            entity.ToTable("warehouse");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
