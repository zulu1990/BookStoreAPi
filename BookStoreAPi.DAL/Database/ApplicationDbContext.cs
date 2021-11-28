using BookStoreApi.Contracts.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreAPi.DAL.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<PurchaseHistory> PurchaseHistory { get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(x => x.UserId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.PurchaseHistory)
                .WithOne(c => c.User)
                .HasForeignKey<PurchaseHistory>(x => x.UserId);


            modelBuilder.Entity<Cart>()
                .HasMany(c => c.Books)
                .WithOne(b => b.Cart);


            modelBuilder.Entity<PurchaseHistory>()
                .HasMany(c => c.Books)
                .WithOne(b => b.PurchaseHistory);

            modelBuilder.Entity<Book>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<User>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<PurchaseHistory>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Cart>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();
        }

    }
}
