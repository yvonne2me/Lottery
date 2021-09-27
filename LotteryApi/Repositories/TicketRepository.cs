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
    }
    
}