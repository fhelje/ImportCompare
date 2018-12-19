using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class EnvironmentsController : ControllerBase {
        private readonly EnvironmentsConfiguration _config;

        public EnvironmentsController(EnvironmentsConfiguration config) {
            _config = config;
        }
        // GET api/values
        // GET api/importlog/environment/5
        [HttpGet("")]
        public ActionResult<string> Get() {
            return Ok(_config.Environments.Select(x => x.Name).ToArray());
        }
    }
}