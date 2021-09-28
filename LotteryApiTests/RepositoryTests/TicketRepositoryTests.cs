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
using System.Collections.Generic;
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
            var saveNewTicket = CreateTicket();

            //Act
            var response = await sut.SaveTicket(saveNewTicket);

            //Assert
            Assert.Equal(saveNewTicket.Id, response.Id);
        }

        [Fact]
        public async void TicketRepository_UpdateTicket_Success()
        {
            //Assign
            SetupTestInfo();
            var sut = new TicketRepository(mockLogger.Object, _context);
            var saveNewTicket = CreateTicket();

            await sut.SaveTicket(saveNewTicket);

            List<Line> lines = new List<Line>();

            Line line = new Line();
            line.Id = Guid.NewGuid();
            line.Numbers = "0, 0, 0";           
            lines.Add(line);

            Ticket updatedTicket = new Ticket()
            {
                Id = saveNewTicket.Id,
                Lines = lines
            };

            var numberOfLines = 2;

            //Act
            var response = await sut.UpdateTicket(updatedTicket, numberOfLines);

            //Assert
            Assert.Equal(saveNewTicket.Id, response.Id);
        }

        [Fact]
        public async void TicketRepository_UpdateTicket_TicketDoesNotExist_Success()
        {
            //Assign
            SetupTestInfo();
            var sut = new TicketRepository(mockLogger.Object, _context);

            var updatedTicketId = Guid.NewGuid();

            List<Line> lines = new List<Line>();

            Line line = new Line();
            line.Id = Guid.NewGuid();
            line.TicketId = updatedTicketId;
            line.Numbers = "1, 0, 2";           
            lines.Add(line);

            Ticket updatedTicket = new Ticket()
            {
                Id = updatedTicketId,
                Lines = lines
            };

            var numberOfLines = 4;
            //Act
            var response = await sut.UpdateTicket(updatedTicket, numberOfLines);

            //Assert
            Assert.Equal(updatedTicketId, response.Id);
        }

        [Fact]
        public async void TicketRepository_GetTicket_ReturnsTicket()
        {
            //Assign
            SetupTestInfo();
            var sut = new TicketRepository(mockLogger.Object, _context);
            var saveNewTicket = CreateTicket();
            
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
                await sut.SaveTicket(CreateTicket());
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

        private Ticket CreateTicket()
        {
            Guid ticketId = Guid.NewGuid();    
            Ticket newTicket = new Ticket()
            {
                Id = ticketId,
                Lines = CreateLines(ticketId)
            };

            return newTicket;
        }

        private List<Line> CreateLines(Guid ticketId)
        {
            Line line = new Line();
            line.Id = Guid.NewGuid();
            line.TicketId = ticketId;
            line.Numbers = "0, 1, 2";

            List<Line> lines = new List<Line>();

            lines.Add(line);

            return lines;
        }
    }
}