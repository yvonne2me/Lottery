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