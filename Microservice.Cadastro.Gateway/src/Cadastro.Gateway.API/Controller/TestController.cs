using Microsoft.AspNetCore.Mvc;

namespace Cadastro.Gateway.API.Controller
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello World");
        }
    }
}
