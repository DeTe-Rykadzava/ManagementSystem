using System;
using System.Collections.Generic;
using Database.DatabaseCore;
using Database.DataDatabase;
using Microsoft.EntityFrameworkCore;

namespace Database.Context;

public partial class ManagementSystemDatabaseContext : DbContext, IManagementSystemDatabaseContext
{
    public ManagementSystemDatabaseContext(DbContextOptions<ManagementSystemDatabaseContext> options)
        : base(options)
    {
        // Database.EnsureCreated();
    }

    public virtual DbSet<BasketProduct> BasketProducts { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderComposition> OrderCompositions { get; set; }

    public virtual DbSet<OrderPaymentType> OrderPaymentTypes { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<OrderTypeSale> OrderTypeSales { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductPhoto> ProductPhotos { get; set; }

    public virtual DbSet<ProductWarehouse> ProductWarehouses { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserBasket> UserBaskets { get; set; }

    public virtual DbSet<UserInfo> UserInfos { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return base.SaveChangesAsync(cancellationToken);
    }
    
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.UseSerialColumns();
    //     modelBuilder.Entity<BasketProduct>(entity =>
    //     {
    //         entity.HasKey(e => e.Id).HasName("basket_product_pk");
    //
    //         entity.ToTable("basket_product");
    //
    //         entity.HasIndex(e => e.ProductId, "IX_basket_product_product_id");
    //
    //         entity.HasIndex(e => e.UserBasketId, "IX_basket_product_user_basket_id");
    //
    //         entity.Property(e => e.Id).HasColumnName("id");
    //         entity.Property(e => e.ProductId).HasColumnName("product_id");
    //         entity.Property(e => e.UserBasketId).HasColumnName("user_basket_id");
    //
    //         entity.HasOne(d => d.Product).WithMany(p => p.BasketProducts)
    //             .HasForeignKey(d => d.ProductId)
    //             .OnDelete(DeleteBehavior.ClientSetNull)
    //             .HasConstraintName("basket_product_product_id_fk");
    //
    //         entity.HasOne(d => d.UserBasket).WithMany(p => p.BasketProducts)
    //             .HasForeignKey(d => d.UserBasketId)
    //             .OnDelete(DeleteBehavior.ClientSetNull)
    //             .HasConstraintName("basket_product_basket_id_fk");
    //     });
    //
    //     modelBuilder.Entity<Order>(entity =>
    //     {
    //         entity.HasKey(e => e.Id).HasName("order_pk");
    //
    //         entity.ToTable("order");
    //
    //         entity.HasIndex(e => e.PaymentTypeId, "IX_order_payment_type_id");
    //
    //         entity.HasIndex(e => e.StatusId, "IX_order_status_id");
    //
    //         entity.HasIndex(e => e.TypeSaleId, "IX_order_type_sale_id");
    //
    //         entity.HasIndex(e => e.UserId, "IX_order_user_id");
    //
    //         entity.Property(e => e.Id).HasColumnName("id");
    //         entity.Property(e => e.BuyerEmail).HasColumnName("buyer_email");
    //         entity.Property(e => e.Cost).HasColumnName("cost");
    //         if (DatabaseSettings.CurrentSelectedServer == DatabaseServers.Mssql)
    //         {
    //             entity.Property(e => e.CreateDate)
    //                 .HasColumnType("datetime")
    //                 .HasColumnName("create_date");
    //         }
    //         else
    //         {
    //             entity.Property(e => e.CreateDate)
    //                 .HasColumnType("timestamp without time zone")
    //                 .HasColumnName("create_date");
    //         }
    //         entity.Property(e => e.PaymentTypeId).HasColumnName("payment_type_id");
    //         entity.Property(e => e.StatusId).HasColumnName("status_id");
    //         if (DatabaseSettings.CurrentSelectedServer == DatabaseServers.Mssql)
    //         {
    //             entity.Property(e => e.StatusUpdateDate)
    //                 .HasColumnType("datetime")
    //                 .HasColumnName("status_update_date");
    //         }
    //         else
    //         {
    //             entity.Property(e => e.StatusUpdateDate)
    //                 .HasColumnType("timestamp without time zone")
    //                 .HasColumnName("status_update_date");
    //         }
    //
    //         entity.Property(e => e.TypeSaleId).HasColumnName("type_sale_id");
    //         entity.Property(e => e.UserId).HasColumnName("user_id");
    //
    //         entity.HasOne(d => d.PaymentType).WithMany(p => p.Orders)
    //             .HasForeignKey(d => d.PaymentTypeId)
    //             .OnDelete(DeleteBehavior.ClientSetNull)
    //             .HasConstraintName("order_payment_type_fk");
    //
    //         entity.HasOne(d => d.Status).WithMany(p => p.Orders)
    //             .HasForeignKey(d => d.StatusId)
    //             .HasConstraintName("order_status_fk");
    //
    //         entity.HasOne(d => d.TypeSale).WithMany(p => p.Orders)
    //             .HasForeignKey(d => d.TypeSaleId)
    //             .HasConstraintName("order_type_sale_fk");
    //
    //         entity.HasOne(d => d.User).WithMany(p => p.Orders)
    //             .HasForeignKey(d => d.UserId)
    //             .HasConstraintName("order_user_fk");
    //     });
    //
    //     modelBuilder.Entity<OrderComposition>(entity =>
    //     {
    //         entity.HasKey(e => e.Id).HasName("order_composition_pk");
    //
    //         entity.ToTable("order_composition");
    //
    //         entity.HasIndex(e => e.OrderId, "IX_order_composition_order_id");
    //
    //         entity.HasIndex(e => e.ProductId, "IX_order_composition_product_id");
    //
    //         entity.Property(e => e.Id).HasColumnName("id");
    //         entity.Property(e => e.OrderId).HasColumnName("order_id");
    //         entity.Property(e => e.ProductCount).HasColumnName("product_count");
    //         entity.Property(e => e.ProductId).HasColumnName("product_id");
    //
    //         entity.HasOne(d => d.Order).WithMany(p => p.OrderCompositions)
    //             .HasForeignKey(d => d.OrderId)
    //             .HasConstraintName("order_composition_order_fk");
    //
    //         entity.HasOne(d => d.Product).WithMany(p => p.OrderCompositions)
    //             .HasForeignKey(d => d.ProductId)
    //             .HasConstraintName("order_composition_product_fk");
    //     });
    //
    //     modelBuilder.Entity<OrderPaymentType>(entity =>
    //     {
    //         entity.HasKey(e => e.Id).HasName("order_payment_type_pk");
    //
    //         entity.ToTable("order_payment_type");
    //
    //         entity.Property(e => e.Id).HasColumnName("id");
    //         entity.Property(e => e.Type).HasColumnName("type");
    //     });
    //
    //     modelBuilder.Entity<OrderStatus>(entity =>
    //     {
    //         entity.HasKey(e => e.Id).HasName("order_status_pk");
    //
    //         entity.ToTable("order_status");
    //
    //         entity.Property(e => e.Id).HasColumnName("id");
    //         entity.Property(e => e.StatusName).HasColumnName("status_name");
    //     });
    //
    //     modelBuilder.Entity<OrderTypeSale>(entity =>
    //     {
    //         entity.HasKey(e => e.Id).HasName("order_type_sale_pk");
    //
    //         entity.ToTable("order_type_sale");
    //
    //         entity.Property(e => e.Id).HasColumnName("id");
    //         entity.Property(e => e.TypeName).HasColumnName("type_name");
    //     });
    //
    //     modelBuilder.Entity<Product>(entity =>
    //     {
    //         entity.HasKey(e => e.Id).HasName("product_pk");
    //
    //         entity.ToTable("product");
    //
    //         entity.HasIndex(e => e.CategoryId, "IX_product_category_id");
    //
    //         entity.Property(e => e.Id)
    //             .HasColumnName("id");
    //         entity.Property(e => e.CategoryId).HasColumnName("category_id");
    //         entity.Property(e => e.Cost).HasColumnName("cost");
    //         entity.Property(e => e.Description).HasColumnName("description");
    //         entity.Property(e => e.Title).HasColumnName("title");
    //
    //         entity.HasOne(d => d.Category).WithMany(p => p.Products)
    //             .HasForeignKey(d => d.CategoryId)
    //             .HasConstraintName("product_category_fk");
    //     });
    //
    //     modelBuilder.Entity<ProductCategory>(entity =>
    //     {
    //         entity.HasKey(e => e.Id).HasName("product_category_pk");
    //
    //         entity.ToTable("product_category");
    //
    //         entity.Property(e => e.Id).HasColumnName("id");
    //         entity.Property(e => e.CategoryName).HasColumnName("category_name");
    //     });
    //
    //     modelBuilder.Entity<ProductPhoto>(entity =>
    //     {
    //         entity.HasKey(e => e.Id).HasName("product_photo_pk");
    //
    //         entity.ToTable("product_photo");
    //
    //         entity.HasIndex(e => e.ProductId, "IX_product_photo_product_id");
    //
    //         entity.Property(e => e.Id).HasColumnName("id");
    //         entity.Property(e => e.Image).HasColumnName("image");
    //         entity.Property(e => e.ProductId).HasColumnName("product_id");
    //
    //         entity.HasOne(d => d.Product).WithMany(p => p.ProductPhotos)
    //             .HasForeignKey(d => d.ProductId)
    //             .HasConstraintName("product_photo_fk");
    //     });
    //
    //     modelBuilder.Entity<ProductWarehouse>(entity =>
    //     {
    //         entity.HasKey(e => e.Id).HasName("product_warehouse_pk");
    //
    //         entity.ToTable("product_warehouse");
    //
    //         entity.HasIndex(e => e.ProductId, "IX_product_warehouse_product_id");
    //
    //         entity.HasIndex(e => e.WarehouseId, "IX_product_warehouse_warehouse_id");
    //
    //         entity.Property(e => e.Id).HasColumnName("id");
    //         entity.Property(e => e.Count).HasColumnName("count");
    //         entity.Property(e => e.CountReserved).HasColumnName("count_reserved");
    //         entity.Property(e => e.ProductId).HasColumnName("product_id");
    //         entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");
    //
    //         entity.HasOne(d => d.Product).WithMany(p => p.ProductWarehouses)
    //             .HasForeignKey(d => d.ProductId)
    //             .HasConstraintName("product_warehouse_product_fk");
    //
    //         entity.HasOne(d => d.Warehouse).WithMany(p => p.ProductWarehouses)
    //             .HasForeignKey(d => d.WarehouseId)
    //             .HasConstraintName("product_warehouse_warehouse_fk");
    //     });
    //
    //     modelBuilder.Entity<Role>(entity =>
    //     {
    //         entity.HasKey(e => e.Id).HasName("role_pk");
    //
    //         entity.ToTable("role");
    //
    //         entity.Property(e => e.Id).HasColumnName("id");
    //         entity.Property(e => e.RoleName).HasColumnName("role_name");
    //     });
    //
    //     modelBuilder.Entity<User>(entity =>
    //     {
    //         entity.HasKey(e => e.Id).HasName("user_pk");
    //
    //         entity.ToTable("user");
    //
    //         entity.HasIndex(e => e.UserInfoId, "IX_user_user_info_id");
    //
    //         entity.Property(e => e.Id).HasColumnName("id");
    //         entity.Property(e => e.HashPassword).HasColumnName("hash_password");
    //         entity.Property(e => e.Login).HasColumnName("login");
    //         entity.Property(e => e.UserInfoId).HasColumnName("user_info_id");
    //
    //         entity.HasOne(d => d.UserInfo).WithMany(p => p.Users)
    //             .HasForeignKey(d => d.UserInfoId)
    //             .HasConstraintName("user_fk");
    //     });
    //
    //     modelBuilder.Entity<UserBasket>(entity =>
    //     {
    //         entity.HasKey(e => e.Id).HasName("user_basket_pk");
    //
    //         entity.ToTable("user_basket");
    //
    //         entity.Property(e => e.Id).HasColumnName("id");
    //         entity.Property(e => e.UserId).HasColumnName("user_id");
    //
    //         entity.HasOne(d => d.User).WithMany(p => p.UserBaskets)
    //             .HasForeignKey(d => d.UserId)
    //             .OnDelete(DeleteBehavior.ClientSetNull)
    //             .HasConstraintName("user_basket_user_fk");
    //     });
    //
    //     modelBuilder.Entity<UserInfo>(entity =>
    //     {
    //         entity.HasKey(e => e.Id).HasName("user_info_pk");
    //
    //         entity.ToTable("user_info");
    //
    //         entity.HasIndex(e => e.RoleId, "IX_user_info_role_id");
    //
    //         entity.Property(e => e.Id).HasColumnName("id");
    //         entity.Property(e => e.FirstName).HasColumnName("first_name");
    //         entity.Property(e => e.LastName).HasColumnName("last_name");
    //         entity.Property(e => e.Patronymic).HasColumnName("patronymic");
    //         entity.Property(e => e.RoleId).HasColumnName("role_id");
    //
    //         entity.HasOne(d => d.Role).WithMany(p => p.UserInfos)
    //             .HasForeignKey(d => d.RoleId)
    //             .HasConstraintName("user_info_role_fk");
    //     });
    //
    //     modelBuilder.Entity<Warehouse>(entity =>
    //     {
    //         entity.HasKey(e => e.Id).HasName("warehouse_pk");
    //
    //         entity.ToTable("warehouse");
    //
    //         entity.Property(e => e.Id).HasColumnName("id");
    //         entity.Property(e => e.Name).HasColumnName("name");
    //     });
    //
    //     OnModelCreatingPartial(modelBuilder);
    // }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
