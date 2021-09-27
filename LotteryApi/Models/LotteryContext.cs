using Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class LotteryContext : DbContext
    {
         public LotteryContext(DbContextOptions<LotteryContext> options) 
            : base(options){}  

        public DbSet<Ticket> Tickets { get; set; }            
    }
}