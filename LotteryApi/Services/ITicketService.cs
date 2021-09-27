using System;
using System.Threading.Tasks;
using Models.Domain;

namespace Services
{
    public interface ITicketService
    {
        Task<Ticket> CreateTicket(Ticket ticket);
    }
}