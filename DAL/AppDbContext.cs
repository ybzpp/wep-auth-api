using Microsoft.EntityFrameworkCore;
using WebAuth.DAL.Models;

namespace WebAuth.DAL;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<UserAuthModel> AuthUsers { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Auth
        modelBuilder.Entity<UserAuthModel>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<UserAuthModel>()
            .Property(u => u.Username)
            .IsRequired();

        modelBuilder.Entity<UserAuthModel>()
            .Property(u => u.Password)
            .IsRequired();

        modelBuilder.Entity<UserAuthModel>()
            .Property(u => u.Salt)
            .IsRequired();
        
        base.OnModelCreating(modelBuilder);
    }
}