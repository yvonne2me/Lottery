using Xunit;
using Moq;
using Models.Domain;
using Repositories;
using Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace ServiceTests
{
    public class TicketServiceTests
    {

        [Fact]
        public async void TicketService_CreateTicket_AssignsId()
        {
            //Assign
            Ticket ticket = new Ticket()
            {
                NumberOfLines = 4
            };

            Mock<ITicketRepository> mockTicketRepository = new Mock<ITicketRepository>();
            mockTicketRepository.Setup(r => r.SaveTicket(It.IsAny<Ticket>())).ReturnsAsync(ticket);
            var sut = new TicketService(mockTicketRepository.Object);

            //Act
            var ticketResponse = await sut.CreateTicket(ticket);

            //Assert
            Assert.NotEqual(Guid.Empty, ticketResponse.Id);
        }

        [Fact]
        public async void TicketService_GetTicket_ReturnsTicket()
        {
            //Assign
            Guid id = Guid.NewGuid();

            Ticket ticket = new Ticket()
            {
                Id = id,
                NumberOfLines = 10
            };

            Mock<ITicketRepository> mockTicketRepository = new Mock<ITicketRepository>();
            mockTicketRepository.Setup(r => r.GetTicket(It.IsAny<Guid>())).ReturnsAsync(ticket);
            var sut = new TicketService(mockTicketRepository.Object);

            //Act
            var ticketResponse = await sut.GetTicket(id);

            //Assert
            Assert.Equal(ticket.Id, ticketResponse.Id);
        }

        [Fact]
        public async void TicketService_GetAllTickets_ReturnsTickets()
        {
            //Assign
            List<Ticket> listOfTickets = new List<Ticket>();

            for(var i=0; i<4; i++)
            {
                var ticket = new Ticket()
                {
                    NumberOfLines = i
                };

                listOfTickets.Add(ticket);
            }

            Mock<ITicketRepository> mockTicketRepository = new Mock<ITicketRepository>();
            mockTicketRepository.Setup(r => r.GetAllTickets()).ReturnsAsync(listOfTickets);
            var sut = new TicketService(mockTicketRepository.Object);

            //Act
            var ticketResponse = await sut.GetAllTickets();

            //Assert
            Assert.Equal(4, ticketResponse.Count);
        }  
    }
}