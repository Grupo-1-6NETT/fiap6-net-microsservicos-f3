using System.Net;
using Dapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;

namespace API.Functions
{
    public class PostAdicionarContatoFunction
    {
        private readonly ILogger<PostAdicionarContatoFunction> _logger;

        public PostAdicionarContatoFunction(ILogger<PostAdicionarContatoFunction> logger)
        {
            _logger = logger;
        }

        [Function("PostAdicionarContatoFunction")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "contatos")] HttpRequestData req)
        {
            _logger.LogInformation("Recebida uma solicitação para adicionar um contato.");

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var contato = JsonSerializer.Deserialize<Contato>(requestBody);

                if (contato == null || string.IsNullOrWhiteSpace(contato.Nome) ||
                    string.IsNullOrWhiteSpace(contato.Telefone) ||
                    string.IsNullOrWhiteSpace(contato.DDD) ||
                    string.IsNullOrWhiteSpace(contato.Email))
                {
                    var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                    await badRequestResponse.WriteStringAsync("Dados inválidos para inserção. Todos os campos são obrigatórios.");
                    return badRequestResponse;
                }

                string connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    _logger.LogError("String de conexão com o banco de dados não configurada.");
                    var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                    await errorResponse.WriteStringAsync("Erro de configuração do servidor.");
                    return errorResponse;
                }

                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                string insertQuery = @"
                    INSERT INTO Contatos (ID ,Nome, Telefone, Ddd, Email) 
                    VALUES (NEWID(), @Nome, @Telefone, @Ddd, @Email);
                    SELECT CAST(SCOPE_IDENTITY() as int)";

                var id = await connection.ExecuteScalarAsync<int>(insertQuery, contato);

                var response = req.CreateResponse(HttpStatusCode.Created);
                await response.WriteAsJsonAsync(new
                {
                    Message = "Contato adicionado com sucesso.",
                    Id = id
                });
                return response;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Erro ao desserializar o corpo da requisição.");
                var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await errorResponse.WriteStringAsync("Formato JSON inválido.");
                return errorResponse;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao acessar o banco de dados.");
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync("Erro ao processar a requisição.");
                return errorResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado.");
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync("Erro inesperado ao processar a requisição.");
                return errorResponse;
            }
        }
    }

    public class Contato
    {
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string DDD { get; set; }
        public string Email { get; set; }
    }
}