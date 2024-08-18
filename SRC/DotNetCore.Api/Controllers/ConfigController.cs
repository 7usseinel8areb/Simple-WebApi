using DotNetCore.Domain.RepositoriesInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly IConfigrationsRepository _configrations;

        public ConfigController(IConfigrationsRepository configrations)
        {
            _configrations = configrations;
        }
        [HttpGet]
        [Route("GetConfigurations")]
        public IActionResult GetConfig()
        {
            var allowedHosts = _configrations.GetConfigurtations();
            return StatusCode(StatusCodes.Status200OK,allowedHosts);
        }
    }
}
