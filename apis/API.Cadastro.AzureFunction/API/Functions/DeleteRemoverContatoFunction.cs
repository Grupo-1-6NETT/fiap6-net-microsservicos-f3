using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace API.Functions
{
    public class DeleteRemoverContatoFunction
    {
        private readonly ILogger<DeleteRemoverContatoFunction> _logger;

        public DeleteRemoverContatoFunction(ILogger<DeleteRemoverContatoFunction> logger)
        {
            _logger = logger;
        }

        [Function("DeleteRemoverContatoFunction")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
