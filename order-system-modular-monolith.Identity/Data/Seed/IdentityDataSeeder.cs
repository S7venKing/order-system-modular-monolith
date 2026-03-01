using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace order_system_modular_monolith.Identity.Data.Seed;

using order_system_modular_monolith.BuildingBlocks.Constants;
using order_system_modular_monolith.BuildingBlocks.EFCore;
using order_system_modular_monolith.Identity.Models;
using System.Linq;

public class IdentityDataSeeder : IDataSeeder
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IdentityContext _identityContext;

    public IdentityDataSeeder(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IdentityContext identityContext
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _identityContext = identityContext;
    }

    public async Task SeedAllAsync()
    {
        var pendingMigrations = await _identityContext.Database.GetPendingMigrationsAsync();

        if (!pendingMigrations.Any())
        {
            await SeedRoles();
            await SeedUsers();
        }
    }

    private async Task SeedRoles()
    {
        if (!await _identityContext.Roles.AnyAsync())
        {
            if (await _roleManager.RoleExistsAsync(IdentityConstant.Role.Admin) == false)
            {
                await _roleManager.CreateAsync(new Role { Name = IdentityConstant.Role.Admin });
            }

            if (await _roleManager.RoleExistsAsync(IdentityConstant.Role.User) == false)
            {
                await _roleManager.CreateAsync(new Role { Name = IdentityConstant.Role.User });
            }
        }
    }

    private async Task SeedUsers()
    {
        if (!await _identityContext.Users.AnyAsync())
        {
            if (await _userManager.FindByNameAsync("samh") == null)
            {
                var result = await _userManager.CreateAsync(InitialData.Users.First(), "Admin@123456");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(InitialData.Users.First(), IdentityConstant.Role.Admin);

                }
            }

            if (await _userManager.FindByNameAsync("meysamh2") == null)
            {
                var result = await _userManager.CreateAsync(InitialData.Users.Last(), "User@123456");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(InitialData.Users.Last(), IdentityConstant.Role.User);
                }
            }
        }
    }
}