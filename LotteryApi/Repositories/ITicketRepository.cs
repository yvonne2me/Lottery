using System;
using System.Threading.Tasks;
using Models.Domain;

namespace Repositories
{
    public interface ITicketRepository
    {
        Task<Ticket> SaveTicket(Ticket ticket);
    }
}