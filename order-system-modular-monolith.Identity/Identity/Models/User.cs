using Microsoft.AspNetCore.Identity;

namespace order_system_modular_monolith.Identity.Models;

using System;
using static order_system_modular_monolith.BuildingBlocks.Domain.Behaviors;

public class User : IdentityUser<Guid>, IVersioned
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string PassPortNumber { get; init; }
    public long Version { get; set; }
}