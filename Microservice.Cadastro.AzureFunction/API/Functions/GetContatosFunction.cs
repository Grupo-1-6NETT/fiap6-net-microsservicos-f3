using System.Net;
using Dapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace API.Functions
{
    public class GetContatosFunction
    {
        private readonly ILogger<GetContatosFunction> _logger;

        public GetContatosFunction(ILogger<GetContatosFunction> logger)
        {
            _logger = logger;
        }

        [Function("GetContatosFunction")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "contatos")] HttpRequestData req)
        {
            _logger.LogInformation("Recebida uma solicitação para buscar contatos.");

            string connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");

            using var connection = new SqlConnection(connectionString);
            var contatos = await connection.QueryAsync("SELECT * FROM Contatos");

            // Criando resposta
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await response.WriteStringAsync(JsonSerializer.Serialize(contatos));

            return response;
        }
    }
}
