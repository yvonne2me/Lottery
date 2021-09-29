using Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class LotteryContext : DbContext
    {
         public LotteryContext(DbContextOptions<LotteryContext> options) 
            : base(options){}  

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Line> Lines { get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>()
                .HasMany(c => c.Lines);
        }
    }
}