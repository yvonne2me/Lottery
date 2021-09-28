using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models.API;
using Models.Domain;
using Repositories;

namespace Services
{
    public class TicketService : ITicketService
    {
        ILineService lineService;
        ITicketRepository ticketRepository;

        public TicketService(ILineService lineService, ITicketRepository ticketRepository)
        {
            this.lineService = lineService;
            this.ticketRepository = ticketRepository;
        }

        public async Task<Ticket> CreateTicket(TicketRequest ticketRequest)
        {
            Ticket ticket = new Ticket()
            {
                Id = Guid.NewGuid()
            };

            ticket.Lines = this.lineService.CreateLines(ticket.Id, ticketRequest.NumberOfLines);

            return await this.ticketRepository.SaveTicket(ticket);
        }

        public async Task<Ticket> UpdateTicket(Guid id, TicketRequest ticketRequest)
        {
            //TODO: IF Existing Ticket Does Not Exist
            var existingTicket = await this.ticketRepository.GetTicket(id);

            return await this.ticketRepository.UpdateTicket(existingTicket, ticketRequest.NumberOfLines);
        }

        public async Task<Ticket> GetTicket(Guid id)
        {
            return await this.ticketRepository.GetTicket(id);
        }

        public async Task<List<Ticket>> GetAllTickets()
        {
            return await this.ticketRepository.GetAllTickets();
        }
    }
}