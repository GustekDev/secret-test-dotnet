using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class GetSecret
    {
        private readonly ILogger<GetSecret> _logger;

        public GetSecret(ILogger<GetSecret> logger)
        {
            _logger = logger;
        }

        [Function("GetSecret")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("Start");
            var secretClient = new SecretClient(vaultUri: new Uri("https://gustek-secret-vault.vault.azure.net/"), credential: new ManagedIdentityCredential());
            _logger.LogInformation("Getting secret");
            var secret = await secretClient.GetSecretAsync("test-secret");
            _logger.LogInformation("Got the secret");
            return new OkObjectResult($"The secret is {secret.Value.Value}");
        }
    }
}