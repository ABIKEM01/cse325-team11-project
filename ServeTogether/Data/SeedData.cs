using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServeTogether.Models;

namespace ServeTogether.Data;

/// <summary>
/// Runs at startup: applies migrations, creates the Leader role, and (if configured
/// in user secrets) a leader account plus one sample project to work with.
/// Any signed-in user without the Leader role is treated as a Member.
/// </summary>
public static class SeedData
{
    public const string LeaderRole = "Leader";

    public static async Task InitializeAsync(IServiceProvider services, IConfiguration config)
    {
        var db = services.GetRequiredService<ApplicationDbContext>();
        await db.Database.MigrateAsync();

        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        if (!await roleManager.RoleExistsAsync(LeaderRole))
        {
            await roleManager.CreateAsync(new IdentityRole(LeaderRole));
        }

        // Leader credentials come from user secrets, never from the repo
        var email = config["SeedLeader:Email"];
        var password = config["SeedLeader:Password"];
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            return;
        }

        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var leader = await userManager.FindByEmailAsync(email);
        if (leader is null)
        {
            leader = new ApplicationUser { UserName = email, Email = email, EmailConfirmed = true };
            var result = await userManager.CreateAsync(leader, password);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    "Could not create the seed leader: " +
                    string.Join("; ", result.Errors.Select(e => e.Description)));
            }
        }

        if (!await userManager.IsInRoleAsync(leader, LeaderRole))
        {
            await userManager.AddToRoleAsync(leader, LeaderRole);
        }

        if (!await db.ServiceProjects.AnyAsync())
        {
            var day = DateTime.Today.AddDays(7);
            db.ServiceProjects.Add(new ServiceProject
            {
                Title = "Community Park Clean-Up",
                Description = "Pick up litter, rake leaves, and repaint the benches. Gloves and bags provided.",
                Location = "City Park, main entrance",
                Date = day,
                CreatedById = leader.Id,
                Slots =
                [
                    new Slot { StartTime = day.AddHours(8), EndTime = day.AddHours(10), Capacity = 6 },
                    new Slot { StartTime = day.AddHours(10), EndTime = day.AddHours(12), Capacity = 4 }
                ]
            });
            await db.SaveChangesAsync();
        }
    }
}
