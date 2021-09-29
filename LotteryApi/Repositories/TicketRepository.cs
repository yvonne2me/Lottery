using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logging;
using Models;
using Models.Domain;
using Microsoft.EntityFrameworkCore;
using Builders;

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
            foreach(var line in ticket.Lines)
            {
                _context.Lines.Add(line);  
            }

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

        public async Task<Ticket> UpdateTicket(Ticket existingTicket, int numberOfLines)
        {
            var newLines = LineBuilder.CreateLines(existingTicket.Id, numberOfLines);

            foreach(var newLine in newLines)
            {
                _context.Entry(newLine).State = EntityState.Added;
            }

            if(await _context.SaveChangesAsync() > 0)
            {
                return existingTicket;
            }
            else
            {
                logger.LogError("TicketRepository - UpdateTicket - Unable to Update Ticket");
                throw new Exception("Error updating Ticket");
            }
        }

        public async Task<Ticket> GetTicket(Guid id)
        {
            return await _context.Tickets.Where(t => t.Id.Equals(id))
                                .Include(t => t.Lines.Where(l => l.TicketId.Equals(id)))
                                .FirstOrDefaultAsync();
        }

        public async Task<List<Ticket>> GetAllTickets()
        {
            return await _context.Tickets.Include(t => t.Lines).ToListAsync();
        }

        public async Task<bool> StatusChecked(Ticket ticket)
        {
            ticket.Checked = true;
            
            _context.Entry(ticket).State = EntityState.Modified;

            if(await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            else
            {
                logger.LogError("TicketRepository - StatusChecked - Unable to set status to checked on Ticket");
                throw new Exception("Error updating checked status on Ticket");
            }
        }
    }
    
}