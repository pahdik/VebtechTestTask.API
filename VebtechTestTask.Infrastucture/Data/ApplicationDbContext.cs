using Microsoft.EntityFrameworkCore;
using VebtechTestTask.Domain.Entities;
using VebtechTestTask.Domain.Enums;

namespace VebtechTestTask.Infrastucture.Data;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public virtual DbSet<User>  Users { get; set; }
    public virtual DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureUserEntity(modelBuilder);
        ConfigureRoleEntity(modelBuilder);
    }

    private void ConfigureUserEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.UserName)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity(j => j.ToTable("UserRole"));

        modelBuilder.Entity<User>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();
    }

    private void ConfigureRoleEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>()
            .Property(r => r.Type)
            .HasConversion(rt => (int)rt,
                rt => (RoleType)rt);
    }
}
