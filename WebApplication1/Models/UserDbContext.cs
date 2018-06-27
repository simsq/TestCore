using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class UserDbContext : DbContext
    {

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        { }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Blog>()
                .Property(x => x.CreateTime);
            modelBuilder.Entity<Blog>()
               .Property(x => x.Timestamp).IsRowVersion();

            base.OnModelCreating(modelBuilder);
        }
    }

    public class Blog
    {
        public int Id { get; set; }

        [MaxLength(20)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Info { get; set; }

        public DateTime CreateTime { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }

    [Table("priducts")]
    public class Product
    {
        public int Id { get; set; }

        [MaxLength(20)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Info { get; set; }

        public int CategoryId { get; set; }

        public virtual Category  Category { get; set; }


    }

    public class Category
    {
        public int Id { get; set; }

        [MaxLength(20)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Info { get; set; }

        public ICollection<Product> ProductsList { get; set; }

    }

}
