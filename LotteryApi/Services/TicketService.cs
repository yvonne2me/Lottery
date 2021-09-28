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

        public async Task<Ticket> UpdateTicket(Guid id, Ticket ticket)
        {
            var existingTicket = await this.ticketRepository.GetTicket(id);

            if(existingTicket == null)
            {
                existingTicket.Id = id;
                return await this.ticketRepository.SaveTicket(ticket);
            }

            ticket.Id = id;
            return await this.ticketRepository.UpdateTicket(ticket);
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