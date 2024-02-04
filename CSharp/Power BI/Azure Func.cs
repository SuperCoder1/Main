
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using System;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(YourNamespace.Startup))]

namespace YourNamespace
{
    public class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var config = builder.ConfigurationBuilder.Build();
            builder.ConfigurationBuilder.AddAzureKeyVault(
                config["AzureKeyVaultUrl"],
                config["ClientId"],
                config["ClientSecret"]
            );

	//Alternatively use this:
	 builder.Configuration.AddAzureKeyVault( new Uri($"https://{builder.Configuration["KeyVaultName"]}.vault.azure.net/"), new DefaultAzureCredential());

        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<ITokenProvider, TokenProvider>();
        }
    }

    public interface ITokenProvider
    {
        Task<string> GetAccessToken();
    }

    public class TokenProvider : ITokenProvider
    {
        private readonly IConfiguration _configuration;

        public TokenProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetAccessToken()
        {
            var clientId = _configuration["ClientId"];
            var tenantId = _configuration["TenantId"];
            var clientSecret = _configuration["ClientSecret"];

            var app = ConfidentialClientApplicationBuilder
                .Create(clientId)
                .WithClientSecret(clientSecret)
	   //.WithTenantId(tenantId)
                .WithAuthority($"https://login.microsoftonline.com/{tenantId}")
                .Build();

            var scopes = new[] { "https://analysis.windows.net/powerbi/api/.default" };

            var result = await app.AcquireTokenForClient(scopes).ExecuteAsync();

            return result.AccessToken;
        }
    }

    public class YourFunction
    {
        private readonly ITokenProvider _tokenProvider;

        public YourFunction(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        [FunctionName("YourFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var accessToken = await _tokenProvider.GetAccessToken();

            // Use the access token to interact with Power BI API or embed Power BI report

            return new OkObjectResult("Success");
        }
    }
}
