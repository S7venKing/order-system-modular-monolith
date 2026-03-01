namespace order_system_modular_monolith.Identity.Models;

using System;
using Microsoft.AspNetCore.Identity;
using static order_system_modular_monolith.BuildingBlocks.Domain.Behaviors;

public class RoleClaim : IdentityRoleClaim<Guid>, IVersioned
{
    public long Version { get; set; }
}