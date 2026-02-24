using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace order_system_modular_monolith.BuildingBlocks.Web
{
    using System.Security.Claims;


    public interface ICurrentUserProvider
    {
        long? GetCurrentUserId();
    }

    public class CurrentUserProvider : ICurrentUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public long? GetCurrentUserId()
        {
            var nameIdentifier = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

            long.TryParse(nameIdentifier?.Value, out var userId);

            return userId;
        }
    }
}
