using ContactListService.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactListService.Data;

/// <summary>
/// Database context for the application
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Contact> Contacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>()
            .HasIndex(c => c.FirstName);
        modelBuilder.Entity<Contact>()
            .HasIndex(c => c.LastName);

        base.OnModelCreating(modelBuilder);
    }
}