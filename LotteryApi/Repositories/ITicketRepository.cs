using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Domain;

namespace Repositories
{
    public interface ITicketRepository
    {
        Task<Ticket> SaveTicket(Ticket ticket);
        Task<Ticket> UpdateTicket(Ticket ticket, int numberOfLines);
        Task<Ticket> GetTicket(Guid id);
        Task<List<Ticket>> GetAllTickets();
    }
}