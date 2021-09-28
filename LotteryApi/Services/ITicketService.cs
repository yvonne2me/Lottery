using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Models.API;
using Models.Domain;

namespace Services
{
    public interface ITicketService
    {
        Task<Ticket> CreateTicket(TicketRequest ticketRequest);
        Task<Ticket> UpdateTicket(Guid id, TicketRequest ticketRequest);
        Task<Ticket> GetTicket(Guid id);
        Task<List<Ticket>> GetAllTickets();
    }
}