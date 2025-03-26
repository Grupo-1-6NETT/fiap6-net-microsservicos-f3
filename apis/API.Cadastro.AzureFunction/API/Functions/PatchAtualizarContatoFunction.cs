using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace API.Functions
{
    public class PatchAtualizarContatoFunction
    {
        private readonly ILogger<PatchAtualizarContatoFunction> _logger;

        public PatchAtualizarContatoFunction(ILogger<PatchAtualizarContatoFunction> logger)
        {
            _logger = logger;
        }

        [Function("PatchAtualizarContatoFunction")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
