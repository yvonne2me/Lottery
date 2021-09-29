using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Domain;
using Repositories;
using Logging;
using System;
using Builders;

namespace RepositoryTests
{
    public class TicketRepositoryTests
    {
        LotteryContext context;
        Mock<IFileLogger> mockLogger;
        Ticket newTicket;

        [Fact]
        public async void TicketRepository_SaveTicket_Success()
        {
            //Assign
            SetupMocksAndTestData();
            var sut = new TicketRepository(mockLogger.Object, context);

            //Act
            var response = await sut.SaveTicket(newTicket);

            //Assert
            Assert.Equal(newTicket.Id, response.Id);
        }

        [Fact]
        public async void TicketRepository_UpdateTicket_Success()
        {
            //Assign
            SetupMocksAndTestData();
            var sut = new TicketRepository(mockLogger.Object, context);
            var updatedTicketId = Guid.NewGuid();
            var numberOfLines = 4;

            Ticket updatedTicket = new Ticket(){ Id = updatedTicketId };

            //Act
            var response = await sut.UpdateTicket(updatedTicket, numberOfLines);

            //Assert
            Assert.Equal(updatedTicketId, response.Id);
        }

        [Fact]
        public async void TicketRepository_GetTicket_ReturnsTicket()
        {
            //Assign
            SetupMocksAndTestData();
            var sut = new TicketRepository(mockLogger.Object, context);
            await sut.SaveTicket(this.newTicket);

            //Act
            var response = await sut.GetTicket(this.newTicket.Id);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(newTicket.Id, response.Id);
        }

        [Fact]
        public async void TicketRepository_GetAllTickets_ReturnsTickets()
        {
            //Assign
            SetupMocksAndTestData();
            var sut = new TicketRepository(mockLogger.Object, context);
            var ticketsToCreate = 2;

            for(var i=0; i<ticketsToCreate; i++)
            {
                await sut.SaveTicket(CreateTicket());
            }          

            //Act
            var response = await sut.GetAllTickets();

            //Assert
            Assert.NotNull(response);
        }

        [Fact]
        public async void TicketRepository_StatusChecked_ReturnsTrue()
        {
            //Assign
            SetupMocksAndTestData();
            var sut = new TicketRepository(mockLogger.Object, context);
            await sut.SaveTicket(this.newTicket);

            //Act
            var response = await sut.StatusChecked(this.newTicket);

            //Assert
            Assert.True(response);
        }

        private void SetupMocksAndTestData()
        {
            this.mockLogger = new Mock<IFileLogger>();
            var builder = new DbContextOptionsBuilder<LotteryContext>()
                    .UseInMemoryDatabase("LotteryContext");

            this.context = new LotteryContext(builder.Options);
            this.newTicket = CreateTicket();
        }

        private Ticket CreateTicket()
        {
            Guid ticketId = Guid.NewGuid();    
            return new Ticket()
            {
                Id = ticketId,
                Lines = LineBuilder.CreateLines(ticketId, 3)
            };
        }
    }
}