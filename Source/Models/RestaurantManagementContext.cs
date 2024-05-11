using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RestaurantManagement.Models;

namespace RestaurantManagement.Models
{
    public partial class RestaurantManagementContext : DbContext
    {
        public RestaurantManagementContext()
        {
        }

        public RestaurantManagementContext(DbContextOptions<RestaurantManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Combo> Combos { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Food> Foods { get; set; } = null!;
        public virtual DbSet<FoodCategory> FoodCategories { get; set; } = null!;
        public virtual DbSet<FoodCombo> FoodCombos { get; set; } = null!;
        public virtual DbSet<FoodTable> FoodTables { get; set; } = null!;
        public virtual DbSet<TableOrder> TableOrders { get; set; } = null!;
        public virtual DbSet<TableOrderCustomer> TableOrderCustomers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                string connectionString = config.GetConnectionString("MyDB");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Combo>(entity =>
            {
                entity.ToTable("Combo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Price)
                    .HasColumnType("money")
                    .HasColumnName("price");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Fullname)
                    .HasMaxLength(10)
                    .HasColumnName("fullname")
                    .IsFixedLength();

                entity.Property(e => e.Password)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Phone).HasColumnName("phone");

                entity.Property(e => e.Username)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<Food>(entity =>
            {
                entity.ToTable("Food");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Image).HasColumnName("image");

                entity.Property(e => e.Price)
                    .HasColumnType("money")
                    .HasColumnName("price");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Foods)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Food_FoodCategory");
            });

            modelBuilder.Entity<FoodCategory>(entity =>
            {
                entity.ToTable("FoodCategory");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<FoodCombo>(entity =>
            {
                entity.ToTable("FoodCombo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ComboId).HasColumnName("combo_id");

                entity.Property(e => e.FoodId).HasColumnName("food_id");

                entity.HasOne(d => d.Combo)
                    .WithMany(p => p.FoodCombos)
                    .HasForeignKey(d => d.ComboId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FoodCombo_Combo");

                entity.HasOne(d => d.Food)
                    .WithMany(p => p.FoodCombos)
                    .HasForeignKey(d => d.FoodId)
                    .HasConstraintName("FK_FoodCombo_Food");
            });

            modelBuilder.Entity<FoodTable>(entity =>
            {
                entity.ToTable("FoodTable");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ComboId).HasColumnName("combo_id");

                entity.Property(e => e.FoodId).HasColumnName("food_id");

                entity.Property(e => e.TableOrderCustomerId).HasColumnName("table_order_customer_id");

                entity.HasOne(d => d.Food)
                    .WithMany(p => p.FoodTables)
                    .HasForeignKey(d => d.FoodId)
                    .HasConstraintName("FK_FoodTable_Food");

                entity.HasOne(d => d.TableOrderCustomer)
                    .WithMany(p => p.FoodTables)
                    .HasForeignKey(d => d.TableOrderCustomerId)
                    .HasConstraintName("FK_FoodTable_TableOrderCustomer");
            });

            modelBuilder.Entity<TableOrder>(entity =>
            {
                entity.ToTable("TableOrder");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Floor).HasColumnName("floor");

                entity.Property(e => e.Number).HasColumnName("number");
            });

            modelBuilder.Entity<TableOrderCustomer>(entity =>
            {
                entity.ToTable("TableOrderCustomer");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.End)
                    .HasColumnType("datetime")
                    .HasColumnName("end");

                entity.Property(e => e.Start)
                    .HasColumnType("datetime")
                    .HasColumnName("start");

                entity.Property(e => e.TableOrderId).HasColumnName("table_order_id");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.TableOrderCustomers)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TableOrderCustomer_Customer");

                entity.HasOne(d => d.TableOrder)
                    .WithMany(p => p.TableOrderCustomers)
                    .HasForeignKey(d => d.TableOrderId)
                    .HasConstraintName("FK_TableOrderCustomer_TableOrder");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
