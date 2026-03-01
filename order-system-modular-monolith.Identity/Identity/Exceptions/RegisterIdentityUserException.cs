
using order_system_modular_monolith.BuildingBlocks.Exceptions;

namespace Identity.Identity.Exceptions;

public class RegisterIdentityUserException : AppException
{
    public RegisterIdentityUserException(string code, string message) : base(code, message)
    {
    }
}