using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Logging;
using Services;
using Models.API;
using AutoMapper;
using Exceptions;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {
        private readonly IFileLogger logger;
        private IMapper mapper;

        private IStatusService statusService;

        public StatusController(IFileLogger logger, IMapper mapper, IStatusService statusService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.statusService = statusService;
        }

        [HttpPut]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Put(Guid id)
        {
            Status response = null;

            try
            {
                response = await this.statusService.GetTicketResult(id);
            }
            catch(TicketNotFoundException ticketNotFoundException)
            {
                return NotFound(ticketNotFoundException.Message);
            }
            catch(Exception)
            {
                throw new Exception("Error occurred while checking Status");
            }

            return Ok(response);
        }
    }
}
