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
            Ticket ticket = new Ticket()
            {
                Id = Guid.NewGuid()
            };

            ticket.Lines = LineBuilder.CreateLines(ticket.Id, ticketRequest.NumberOfLines);

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
    }
}