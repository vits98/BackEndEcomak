using Ecomak.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecomak.Data
{
    public class EcomakDbContext : IdentityDbContext
    {
        public EcomakDbContext(DbContextOptions<EcomakDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           

            modelBuilder.Entity<CategoryEntity>().ToTable("Categories");
            modelBuilder.Entity<CategoryEntity>().HasMany(a => a.Products).WithOne(b => b.Category);
            modelBuilder.Entity<CategoryEntity>().HasMany(a => a.Trs).WithOne(b => b.Category);
            modelBuilder.Entity<CategoryEntity>().Property(a => a.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<TrEntity>().ToTable("Trs");
            modelBuilder.Entity<TrEntity>().Property(b => b.IdTr).ValueGeneratedOnAdd();
            modelBuilder.Entity<TrEntity>().HasOne(b => b.Category).WithMany(a => a.Trs);
            modelBuilder.Entity<TrEntity>().HasMany(b => b.Quotes).WithOne(a => a.Tr);

            modelBuilder.Entity<ProductEntity>().ToTable("Products");
            modelBuilder.Entity<ProductEntity>().Property(b => b.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ProductEntity>().HasOne(b => b.Category).WithMany(a => a.Products);
            modelBuilder.Entity<ProductEntity>().HasMany(p => p.Quotes).WithOne(q => q.Product);

            modelBuilder.Entity<PromotionEntity>().ToTable("Promotions");
            modelBuilder.Entity<PromotionEntity>().HasMany(a => a.Comments).WithOne(b => b.Promotion);
            modelBuilder.Entity<PromotionEntity>().Property(a => a.id).ValueGeneratedOnAdd();

            modelBuilder.Entity<CommentaryEntity>().ToTable("Comments");
            modelBuilder.Entity<CommentaryEntity>().Property(b => b.id).ValueGeneratedOnAdd();
            modelBuilder.Entity<CommentaryEntity>().HasOne(b => b.Promotion).WithMany(a => a.Comments);

            modelBuilder.Entity<QuoteEntity>().ToTable("Quotes");
            modelBuilder.Entity<QuoteEntity>().Property(q => q.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<QuoteEntity>().HasOne(q => q.Tr).WithMany(a => a.Quotes);
            modelBuilder.Entity<QuoteEntity>().HasOne(q => q.Product).WithMany(a => a.Quotes);
            

            modelBuilder.Entity<SubscribeEntity>().ToTable("Subscribes");
            modelBuilder.Entity<SubscribeEntity>().Property(a => a.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<ImageEntity>().ToTable("Images");
            modelBuilder.Entity<ImageEntity>().Property(i => i.Id).ValueGeneratedOnAdd();
        }

        public DbSet<TrEntity> Trs { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<PromotionEntity> Promotions { get; set; }
        public DbSet<CommentaryEntity> Comments { get; set; }
        public DbSet<SubscribeEntity> Subscribes { get; set; }
        public DbSet<QuoteEntity> Quotes { get; set; }
        public DbSet<ImageEntity> Images { get; set; }
    }
}
