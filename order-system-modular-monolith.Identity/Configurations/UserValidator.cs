namespace order_system_modular_monolith.Module.Configurations;

using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using Identity.Models;
using Microsoft.AspNetCore.Identity;

public class UserValidator : IResourceOwnerPasswordValidator
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public UserValidator(SignInManager<User> signInManager,
        UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
{
    var user = await _userManager.FindByNameAsync(context.UserName);

    if (user == null)
    {
        context.Result = new GrantValidationResult(
            TokenRequestErrors.InvalidGrant,
            "Invalid username or password");

        return;
    }

    var signIn = await _signInManager.CheckPasswordSignInAsync(
        user,
        context.Password,
        lockoutOnFailure: true);

    if (!signIn.Succeeded)
    {
        context.Result = new GrantValidationResult(
            TokenRequestErrors.InvalidGrant,
            "Invalid username or password");

        return;
    }

    var userId = user.Id.ToString();

    context.Result = new GrantValidationResult(
        subject: userId,
        authenticationMethod: "password",
        claims: new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, user.UserName!)
        });
}
}