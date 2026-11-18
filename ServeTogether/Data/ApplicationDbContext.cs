using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServeTogether.Models;

namespace ServeTogether.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<ServiceProject> ServiceProjects => Set<ServiceProject>();
    public DbSet<Slot> Slots => Set<Slot>();
    public DbSet<SignUp> SignUps => Set<SignUp>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Keep this first: it configures all the Identity tables
        base.OnModelCreating(builder);

        builder.Entity<ServiceProject>()
            .HasOne(p => p.CreatedBy)
            .WithMany()
            .HasForeignKey(p => p.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Slot>()
            .HasOne(s => s.ServiceProject)
            .WithMany(p => p.Slots)
            .HasForeignKey(s => s.ServiceProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<SignUp>()
            .HasOne(s => s.Slot)
            .WithMany(sl => sl.SignUps)
            .HasForeignKey(s => s.SlotId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<SignUp>()
            .HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // A member can't sign up for the same slot twice
        builder.Entity<SignUp>()
            .HasIndex(s => new { s.SlotId, s.UserId })
            .IsUnique();
    }
}
