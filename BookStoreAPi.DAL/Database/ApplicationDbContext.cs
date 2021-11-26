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


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Post>()
        //        .HasOne(x => x.User)
        //        .WithMany(p => p.Posts);

        //    modelBuilder.Entity<Post>()
        //        .Property(p => p.TargetNetworks)
        //        .HasConversion(
        //            v => JsonSerializer.Serialize(v, null),
        //            v => JsonSerializer.Deserialize<List<SocialNetworks>>(v, null),
        //            new ValueComparer<IList<SocialNetworks>>(
        //                (c1, c2) => c1.SequenceEqual(c2),
        //                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
        //                c => (IList<SocialNetworks>)c.ToList()));

        //    modelBuilder.Entity<Photo>()
        //        .HasOne(x => x.Post)
        //        .WithMany(p => p.Photos);

        //    modelBuilder.Entity<Post>()
        //        .Property(p => p.Id)
        //        .ValueGeneratedOnAdd();

        //    modelBuilder.Entity<Schedule>()
        //        .Property(p => p.Id)
        //        .ValueGeneratedOnAdd();

        //    modelBuilder.Entity<User>()
        //        .Property(p => p.Id)
        //        .ValueGeneratedOnAdd();

        //}
    }
}
