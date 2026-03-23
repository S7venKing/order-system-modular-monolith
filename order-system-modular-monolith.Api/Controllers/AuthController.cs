using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace order_system_modular_monolith.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AuthController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        // Proxy login to IdentityServer: /connect/token (Resource Owner Password)
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request body is required.");
            }

            var authority = _configuration["Jwt:Authority"] ?? "https://localhost:58551";
            var tokenEndpoint = $"{authority.TrimEnd('/')}/connect/token";

            var clientId = string.IsNullOrWhiteSpace(request.ClientId) ? "client" : request.ClientId;
            var clientSecret = string.IsNullOrWhiteSpace(request.ClientSecret) ? "secret" : request.ClientSecret;

            var form = new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", request.Username),
                new KeyValuePair<string, string>("password", request.Password),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                // Scope phải khớp với cấu hình IdentityServer
                new KeyValuePair<string, string>("scope", "openid profile order-system-modular-monolith")
            };

            var client = _httpClientFactory.CreateClient();
            using var content = new FormUrlEncodedContent(form);

            var response = await client.PostAsync(tokenEndpoint, content);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, body);
            }

            // Trả nguyên response từ IdentityServer (chứa access_token, expires_in, token_type, scope, ...)
            return Content(body, "application/json", Encoding.UTF8);
        }

        // Return current authenticated user's claims for debugging
        [HttpGet("me")]
        [Authorize(Policy = "ApiScope")]
        public IActionResult Me()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            return Ok(claims);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ClientId { get; set; } = "client";
        public string ClientSecret { get; set; } = "secret";
    }
}
