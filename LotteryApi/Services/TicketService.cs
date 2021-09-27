using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Domain;
using Repositories;

namespace Services
{
    public class TicketService : ITicketService
    {
        ITicketRepository ticketRepository;

        public TicketService(ITicketRepository ticketRepository)
        {
            this.ticketRepository = ticketRepository;
        }

        public async Task<Ticket> CreateTicket(Ticket ticket)
        {
            ticket.Id = Guid.NewGuid();

            return await this.ticketRepository.SaveTicket(ticket);
        }
    }
}