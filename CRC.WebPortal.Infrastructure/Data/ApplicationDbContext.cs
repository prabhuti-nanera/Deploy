using Microsoft.EntityFrameworkCore;
using CRC.WebPortal.Domain.Entities;

namespace CRC.WebPortal.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Building> Buildings { get; set; }
    public DbSet<Unit> Units { get; set; }
    public DbSet<UnitType> UnitTypes { get; set; }
    public DbSet<NumberingPattern> NumberingPatterns { get; set; }
    public DbSet<NumberingPatternRow> NumberingPatternRows { get; set; }
    public DbSet<UnitOwnership> UnitOwnerships { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Use a specific schema for CRC Web Portal to avoid conflicts with existing schemas
        modelBuilder.HasDefaultSchema("crc_webportal");

        // Configure User entity for authentication
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Configure Project entity
        modelBuilder.Entity<Project>()
            .HasIndex(p => p.Name)
            .IsUnique();

        // Configure Building entity
        modelBuilder.Entity<Building>()
            .HasOne(b => b.Project)
            .WithMany(p => p.Buildings)
            .HasForeignKey(b => b.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Building>()
            .HasIndex(b => new { b.ProjectId, b.Name })
            .IsUnique();

        // Configure Unit entity
        modelBuilder.Entity<Unit>()
            .HasOne(u => u.Building)
            .WithMany(b => b.Units)
            .HasForeignKey(u => u.BuildingId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Unit>()
            .HasIndex(u => new { u.BuildingId, u.UnitNumber })
            .IsUnique();

        // Configure NumberingPattern entity
        modelBuilder.Entity<NumberingPattern>()
            .HasOne(np => np.Project)
            .WithMany(p => p.NumberingPatterns)
            .HasForeignKey(np => np.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure NumberingPatternRow entity
        modelBuilder.Entity<NumberingPatternRow>()
            .HasOne(npr => npr.NumberingPattern)
            .WithMany(np => np.PatternRows)
            .HasForeignKey(npr => npr.NumberingPatternId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure UnitType entity
        modelBuilder.Entity<UnitType>()
            .HasOne(ut => ut.Project)
            .WithMany(p => p.UnitTypes)
            .HasForeignKey(ut => ut.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UnitType>()
            .HasIndex(ut => new { ut.ProjectId, ut.Name })
            .IsUnique();

        // Configure Unit-UnitType relationship
        modelBuilder.Entity<Unit>()
            .HasOne(u => u.UnitType)
            .WithMany(ut => ut.Units)
            .HasForeignKey(u => u.UnitTypeId)
            .OnDelete(DeleteBehavior.SetNull);

        // Configure UnitOwnership entity
        modelBuilder.Entity<UnitOwnership>()
            .HasOne(uo => uo.Unit)
            .WithOne(u => u.Ownership)
            .HasForeignKey<UnitOwnership>(uo => uo.UnitId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UnitOwnership>()
            .HasIndex(uo => uo.UnitId)
            .IsUnique();

        modelBuilder.Entity<UnitOwnership>()
            .HasIndex(uo => uo.OwnerEmail);
    }
}
