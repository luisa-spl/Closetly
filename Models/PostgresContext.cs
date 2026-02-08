using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Closetly.Models;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbOrder> TbOrders { get; set; }

    public virtual DbSet<TbOrderProduct> TbOrderProducts { get; set; }

    public virtual DbSet<TbPayment> TbPayments { get; set; }

    public virtual DbSet<TbProduct> TbProducts { get; set; }

    public virtual DbSet<TbRating> TbRatings { get; set; }

    public virtual DbSet<TbUser> TbUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost:5432;Database=postgres;Username=postgres;Password=root");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pg_catalog", "adminpack");

        modelBuilder.Entity<TbOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("tb_order_pkey");

            entity.ToTable("tb_order", "closetly_core");

            entity.Property(e => e.OrderId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("order_id");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(30)
                .HasColumnName("order_status");
            entity.Property(e => e.OrderTotalItems).HasColumnName("order_total_items");
            entity.Property(e => e.OrderedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("ordered_at");
            entity.Property(e => e.ReturnDate).HasColumnName("return_date");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.TbOrders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_user");
        });

        modelBuilder.Entity<TbOrderProduct>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId }).HasName("tb_order_product_pkey");

            entity.ToTable("tb_order_product", "closetly_core");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Order).WithMany(p => p.TbOrderProducts)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_op_order");

            entity.HasOne(d => d.Product).WithMany(p => p.TbOrderProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_op_product");
        });

        modelBuilder.Entity<TbPayment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("tb_payment_pkey");

            entity.ToTable("tb_payment", "closetly_core");

            entity.Property(e => e.PaymentId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("payment_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(30)
                .HasColumnName("payment_status");
            entity.Property(e => e.PaymentType)
                .HasMaxLength(30)
                .HasColumnName("payment_type");
            entity.Property(e => e.PaymentValue)
                .HasPrecision(10, 2)
                .HasColumnName("payment_value");

            entity.HasOne(d => d.Order).WithMany(p => p.TbPayments)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_payment_order");
        });

        modelBuilder.Entity<TbProduct>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("tb_product_pkey");

            entity.ToTable("tb_product", "closetly_core");

            entity.Property(e => e.ProductId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("product_id");
            entity.Property(e => e.ProductColor)
                .HasMaxLength(30)
                .HasColumnName("product_color");
            entity.Property(e => e.ProductOccasion)
                .HasMaxLength(30)
                .HasColumnName("product_occasion");
            entity.Property(e => e.ProductSize)
                .HasMaxLength(30)
                .HasColumnName("product_size");
            entity.Property(e => e.ProductStatus)
                .HasMaxLength(30)
                .HasColumnName("product_status");
            entity.Property(e => e.ProductType)
                .HasMaxLength(30)
                .HasColumnName("product_type");
            entity.Property(e => e.ProductValue)
                .HasPrecision(10, 2)
                .HasColumnName("product_value");
        });

        modelBuilder.Entity<TbRating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("tb_rating_pkey");

            entity.ToTable("tb_rating", "closetly_core");

            entity.Property(e => e.RatingId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("rating_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Rate).HasColumnName("rate");

            entity.HasOne(d => d.Order).WithMany(p => p.TbRatings)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_rating_order");
        });

        modelBuilder.Entity<TbUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("tb_user_pkey");

            entity.ToTable("tb_user", "closetly_core");

            entity.HasIndex(e => e.Email, "tb_user_email_key").IsUnique();

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("user_id");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.Phone)
                .HasMaxLength(30)
                .HasColumnName("phone");
            entity.Property(e => e.UserName)
                .HasMaxLength(150)
                .HasColumnName("user_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
