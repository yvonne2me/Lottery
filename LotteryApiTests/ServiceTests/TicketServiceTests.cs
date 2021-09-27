using Xunit;
using Moq;
using Models.Domain;
using Repositories;
using Services;
using System.Threading.Tasks;
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
    }
}