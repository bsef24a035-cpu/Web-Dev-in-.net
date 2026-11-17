using Microsoft.AspNetCore.Identity;
using Qistflow_Semester_Project.Data;

namespace QistFlow.Data;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services, IConfiguration config)
    {
        var roles = services.GetRequiredService<RoleManager<IdentityRole>>();
        var users = services.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var role in new[] { "Admin", "Customer" })
        {
            if (!await roles.RoleExistsAsync(role))
            {
                var created = await roles.CreateAsync(new IdentityRole(role));
                if (!created.Succeeded)
                    throw new InvalidOperationException("Cannot create role " + role + ": " +
                        string.Join("; ", created.Errors.Select(e => e.Description)));
            }
        }

        var admins = await users.GetUsersInRoleAsync("Admin");
        if (admins.Count > 1)
            throw new InvalidOperationException(
                "More than one Admin already exists. Resolve this manually before startup.");
        if (admins.Count == 1)
            return; // Existing admin: NEVER create another.

        // Set these using User Secrets, not source code or committed appsettings.json.
        string? email = config["SeedAdmin:Email"]?.Trim();
        string? password = config["SeedAdmin:Password"];
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            throw new InvalidOperationException(
                "No admin exists. Configure SeedAdmin:Email and SeedAdmin:Password " +
                "via dotnet user-secrets, then restart the app.");

        // Never promote a self-registered customer to admin just because email matches.
        if (await users.FindByEmailAsync(email) is not null)
            throw new InvalidOperationException(
                "SeedAdmin:Email belongs to an existing user. Choose a new admin email.");

        var admin = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var result = await users.CreateAsync(admin, password);
        if (!result.Succeeded)
            throw new InvalidOperationException("Unable to create Admin: " +
                string.Join("; ", result.Errors.Select(e => e.Description)));

        var assignment = await users.AddToRoleAsync(admin, "Admin");
        if (!assignment.Succeeded)
        {
            await users.DeleteAsync(admin);
            throw new InvalidOperationException("Admin role assignment failed: " +
                string.Join("; ", assignment.Errors.Select(e => e.Description)));
        }
    }
}
