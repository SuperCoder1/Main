using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;

[assembly: FunctionsStartup(typeof(YourNamespace.Startup))]

namespace YourNamespace
{
    public class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var config = builder.ConfigurationBuilder.Build();
            builder.ConfigurationBuilder.AddConfiguration(config);
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var serviceProvider = builder.Services.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();

            var clientId = configuration["ClientId"];
            var tenantId = configuration["TenantId"];
            var clientSecret = configuration["ClientSecret"];

            var authority = $"https://login.microsoftonline.com/{tenantId}";
            var scopes = new[] { "https://graph.microsoft.com/.default" };

            var confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(authority)
                .Build();

            builder.Services.AddSingleton(confidentialClientApplication);
        }
    }
}
