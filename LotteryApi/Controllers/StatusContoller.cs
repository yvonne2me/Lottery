using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.API;
using Logging;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {
        private readonly IFileLogger _logger;

        public StatusController(IFileLogger logger)
        {
            _logger = logger;
        }

        [HttpPut]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Put(Guid id)
        {
            return Ok("OK");
        }
    }
}
