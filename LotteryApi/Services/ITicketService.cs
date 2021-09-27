using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Models.Domain;

namespace Services
{
    public interface ITicketService
    {
        Task<Ticket> CreateTicket(Ticket ticket);
        Task<Ticket> GetTicket(Guid id);
        Task<List<Ticket>> GetAllTickets();
    }
}