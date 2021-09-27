using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Domain;
using Repositories;
using Logging;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace RepositoryTests
{
    public class TicketRepositoryTests
    {
        LotteryContext _context;
        Mock<IFileLogger> mockLogger;

        [Fact]
        public async void TicketRepository_SaveTicket_Success()
        {
            //Assign
            SetupTestInfo();
            var sut = new TicketRepository(mockLogger.Object, _context);

            Ticket saveNewTicket = new Ticket()
            {
                Id = Guid.NewGuid(),
                NumberOfLines = 5
            };

            //Act
            var response = await sut.SaveTicket(saveNewTicket);

            //Assert
            Assert.Equal(saveNewTicket.Id, response.Id);
        }

        [Fact]
        public async void TicketRepository_GetTicket_ReturnsTicket()
        {
            //Assign
            SetupTestInfo();
            var sut = new TicketRepository(mockLogger.Object, _context);

            Ticket saveNewTicket = new Ticket()
            {
                Id = Guid.NewGuid(),
                NumberOfLines = 5
            };
            
            var createdTicket = await sut.SaveTicket(saveNewTicket);

            //Act
            var response = await sut.GetTicket(createdTicket.Id);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(createdTicket.Id, response.Id);
        }

        [Fact]
        public async void TicketRepository_GetAllTickets_ReturnsTickets()
        {
            //Assign
            SetupTestInfo();
            var sut = new TicketRepository(mockLogger.Object, _context);

            for(var i=0; i<4; i++)
            {
                var ticket = new Ticket()
                {
                    Id = Guid.NewGuid(),
                    NumberOfLines = i
                };

                await sut.SaveTicket(ticket);
            }          

            //Act
            var response = await sut.GetAllTickets();

            //Assert
            Assert.NotNull(response);
        }


        private void SetupTestInfo()
        {
            mockLogger = new Mock<IFileLogger>();
            var builder = new DbContextOptionsBuilder<LotteryContext>()
                    .UseInMemoryDatabase("LotteryContext");

            _context = new LotteryContext(builder.Options);
        }
    }
}