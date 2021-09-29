using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Models.API;
using Logging;
using Services;
using Exceptions;
using System.Collections.Generic;
using Models.Domain;

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
            TicketResponse response = null;

            try
            {
                var getTicket = await this.ticketService.GetTicket(id);
                response = this.mapper.Map<TicketResponse>(getTicket);

            }
            catch(TicketNotFoundException ticketNotFoundException)
            {
                return NotFound(ticketNotFoundException.Message);
            }
            catch(Exception)
            {
                throw new Exception("Error occurred while getting Ticket");
            }

            return Ok(response);
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get()
        {
            List<TicketResponse> response = null;

            try
            {
                var getAllTickets = await this.ticketService.GetAllTickets();

                response = new List<TicketResponse>();

                foreach(var ticket in getAllTickets)
                {
                    var element = this.mapper.Map<TicketResponse>(ticket);                  
                    response.Add(element);
                }               

            }
            catch(TicketNotFoundException ticketNotFoundException)
            {
                return NotFound(ticketNotFoundException.Message);
            }
            catch(Exception)
            {
                throw new Exception("Error occurred while getting Ticket");
            }

            return Ok(response);
        }
    }
}
