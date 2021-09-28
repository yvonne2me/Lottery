using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Models.API;
using Logging;
using Services;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly IFileLogger logger;
        private IMapper mapper;
        private ITicketService ticketService;

        public TicketController(IFileLogger logger, IMapper mapper, ITicketService ticketService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.ticketService = ticketService;
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

            TicketResponse response = null;

            try
            {
                var createTicketResponse = await this.ticketService.CreateTicket(ticketRequest);
                response = this.mapper.Map<TicketResponse>(createTicketResponse);
            }
            catch(ArgumentException argumentException)
            {
                return BadRequest(argumentException.Message);
            }
            catch(Exception)
            {
                throw new Exception("Error occurred while saving Ticket");
            }

            return Ok(response);
        }

        [HttpPut]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Put(Guid id, [FromBody] TicketRequest ticketRequest)
        {
            if(ticketRequest == null)
            {
                this.logger.LogInfo("Ticket Request info null - Returning Bad Request");
                return BadRequest("No Ticket information provided");
            }

            TicketResponse response = null;

            try
            {
                var updateTicketResponse = await this.ticketService.UpdateTicket(id, ticketRequest);
                response = this.mapper.Map<TicketResponse>(updateTicketResponse);
            }
            catch(ArgumentException argumentException)
            {
                return BadRequest(argumentException.Message);
            }
            catch(Exception)
            {
                throw new Exception("Error occurred while updating Ticket");
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]        
        public async Task<IActionResult> Get(Guid id)
        {
            var response = await this.ticketService.GetTicket(id);

            if(response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get()
        {
            var response = await this.ticketService.GetAllTickets();

            if(response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }
    }
}
