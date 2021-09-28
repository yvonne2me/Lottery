using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logging;
using Models;
using Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class TicketRepository : ITicketRepository
    {
        LotteryContext _context;
        IFileLogger logger;

        public TicketRepository(IFileLogger logger, LotteryContext context)
        {
            _context = context;
            this.logger = logger;
        }

        public async Task<Ticket> SaveTicket(Ticket ticket)
        {
            _context.Tickets.Add(ticket);

            if(await _context.SaveChangesAsync() > 0)
            {
                return ticket;
            }
            else
            {
                logger.LogError("TicketRepository - SaveTicket - Unable to Save Ticket");
                throw new Exception("Error saving new Ticket");
            }
        }

        public async Task<Ticket> UpdateTicket(Ticket ticket)
        {
            var existingTicket = await GetTicket(ticket.Id);

            if(existingTicket != null)
            {
                _context.Tickets.Remove(existingTicket);
                return await SaveTicket(ticket);
            }

            return await SaveTicket(ticket);            
        }

        public async Task<Ticket> GetTicket(Guid id)
        {
            return await _context.Tickets.Where(t => t.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<List<Ticket>> GetAllTickets()
        {
            return await _context.Tickets.ToListAsync();
        }
    }
    
}