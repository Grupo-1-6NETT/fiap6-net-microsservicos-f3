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
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "contatos")] HttpRequestData req)
        {
            var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            string ddd = query["ddd"];
            string id = query["id"];

        
        _logger.LogInformation("Recebida uma solicitação para buscar contatos.");

            string connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
            using var connection = new SqlConnection(connectionString);

            IEnumerable<dynamic> contatos;

            if (!string.IsNullOrEmpty(id)) // Busca por ID
            {
                _logger.LogInformation($"Buscando contato pelo ID: {id}");
                contatos = await connection.QueryAsync("SELECT * FROM Contatos WHERE Id = @Id", new { Id = id });
            }
            else if (!string.IsNullOrEmpty(ddd)) // Busca por DDD
            {
                _logger.LogInformation($"Buscando contatos pelo DDD: {ddd}");
                contatos = await connection.QueryAsync("SELECT * FROM Contatos WHERE DDD = @DDD", new { DDD = ddd });
            }
            else // Retorna todos os contatos
            {
                contatos = await connection.QueryAsync("SELECT * FROM Contatos");
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await response.WriteStringAsync(JsonSerializer.Serialize(contatos));

            return response;
        }
    }
}
