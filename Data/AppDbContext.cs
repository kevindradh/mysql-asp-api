using CobaMySql.Models;
using Microsoft.EntityFrameworkCore;

namespace CobaMySql.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var passwordDefault = BCrypt.Net.BCrypt.HashPassword("password");
        
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "kevind", PasswordHash = passwordDefault, Email = "kevind@example.com", FullName = "Kevind"}, 
            new User { Id = 2, Username = "radhitya", PasswordHash = passwordDefault, Email = "radhitya@example.com", FullName = "Radhitya"},
            new User { Id = 3, Username = "wicaksono", PasswordHash = passwordDefault, Email = "wicaksono@example.com", FullName = "Wicaksono"});

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Electronics",  Description = "Items that operate using electrical power or digital components.", Slug = "electronics"},
            new Category { Id = 2, Name = "Fashion",  Description = "Products related to clothing styles and accessories.", Slug = "fashion"});

        modelBuilder.Entity<Product>().HasData(
            new Product 
            {
                Id = 1,
                Name = "ThinkPad L16 Gen 2 16 Inch AMD",
                Description = "This laptop integrates advanced AI capabilities to improve your workflow through real-time suggestions, personalized insights, and process automation.",
                CategoryId = 1,
                Price = 12000000,
                Stock = 5,
                ImageUrl = "thinkpad-l16.webp"
            },
            new Product
            {
                Id = 2,
                Name = "Diadora Piagio Men's Running Shoes - Black", 
                Description = "Perform with light textile upper and eva outsole for light feeling on easy sports activities", 
                CategoryId = 2,
                Price = 4000000,
                Stock = 5,
                ImageUrl = "diadora-piagio-black.webp"
            });

        modelBuilder.Entity<Product>()
            .HasOne(c => c.Category)
            .WithMany(p => p.Products)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
}