using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.API;
using Models.Domain;
using Logging;


namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly IFileLogger logger;
        private IMapper mapper;

        public TicketController(IFileLogger logger, IMapper mapper)
        {
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> Post([FromBody] TicketRequest ticketRequest)
        {
            if(ticketRequest == null)
            {
                this.logger.LogInfo("Ticket Request info null - Returning Bad Request");
                return BadRequest("No Ticket information provided");
            }

            var createTicketRequest = this.mapper.Map<Ticket>(ticketRequest);

            //Ticket response = null;

            // try
            // {
            //     response = await ticketService.CreateTicket(createTicketRequest);
            // }
            // catch(ArgumentException argumentException)
            // {
            //     return BadRequest(argumentException.Message);
            // }
            // catch(Exception)
            // {
            //     throw new Exception("Error occurred while saving Ticket");
            // }

            return Ok(createTicketRequest);
        }

        [HttpPut]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Put([FromBody] TicketRequest ticketRequest)
        {
            this.logger.LogInfo("PUT existing ticket");

            return Ok("OK");
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(Guid id)
        {
            if(id == Guid.Empty)
            {
                this.logger.LogInfo("GET all tickets...");
            }

            this.logger.LogInfo("GET individual ticket: " + id);

            return Ok("OK");
        }
    }
}
