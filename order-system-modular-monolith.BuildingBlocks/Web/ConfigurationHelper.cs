using Microsoft.Extensions.Configuration;

namespace order_system_modular_monolith.BuildingBlocks.Web
{
    public static class ConfigurationHelper
    {
        public static IConfiguration GetConfiguration(string basePath = null)
        {
            basePath ??= Directory.GetCurrentDirectory();
            var environmentVariable = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentVariable}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}