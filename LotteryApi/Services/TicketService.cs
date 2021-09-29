using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.API;
using Models.Domain;
using Repositories;
using Exceptions;
using Builders;

namespace Services
{
    public class TicketService : ITicketService
    {
        ITicketRepository ticketRepository;

        public TicketService(ITicketRepository ticketRepository)
        {
            this.ticketRepository = ticketRepository;
        }

        public async Task<Ticket> CreateTicket(TicketRequest ticketRequest)
        {
            var ticket = BuildTicket(Guid.NewGuid(), ticketRequest.NumberOfLines);

            return await this.ticketRepository.SaveTicket(ticket);
        }

        public async Task<Ticket> UpdateTicket(Guid id, TicketRequest ticketRequest)
        {
            var existingTicket = await this.ticketRepository.GetTicket(id);

            if(existingTicket == null)
            {
                var newTicket = BuildTicket(id, ticketRequest.NumberOfLines);
                return await this.ticketRepository.SaveTicket(newTicket);
            }

            if(existingTicket.Checked)
            {
                throw new ArgumentException("Ticket already checked and cannot be updated");
            }

            return await this.ticketRepository.UpdateTicket(existingTicket, ticketRequest.NumberOfLines);
        }

        public async Task<Ticket> GetTicket(Guid id)
        {
            var ticket = await this.ticketRepository.GetTicket(id);

            if(ticket == null)
            {
                throw new TicketNotFoundException("Ticket Does Not Exist");
            }

            return ticket;
        }

        public async Task<List<Ticket>> GetAllTickets()
        {
            var tickets = await this.ticketRepository.GetAllTickets();

            if(tickets == null || tickets.Count ==0)
            {
                throw new TicketNotFoundException("No Tickets Found");
            }

            return tickets;
        }

        private Ticket BuildTicket(Guid ticketId, int numberOfLines)
        {
            Ticket ticket = new Ticket()
            {
                Id = ticketId
            };

            ticket.Lines = LineBuilder.CreateLines(ticketId, numberOfLines);
            return ticket;
        }
    }
}