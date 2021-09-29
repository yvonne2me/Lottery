using Xunit;
using Moq;
using Models.Domain;
using Repositories;
using Services;
using System.Collections.Generic;
using System;
using Exceptions;
using System.Threading.Tasks;

namespace ServiceTests
{
    public class StatusServiceTests
    {

        [Fact]
        public async void StatusService_GetTicketResult_SumEqualsTwo_ReturnsTen()
        {
            //Assign
            var expectedResult = 10;

            var ticket = new Ticket() { Id = Guid.NewGuid()};
            ticket.Lines = new List<Line>()
            {
                new Line() { Numbers = "1, 1, 0"}
            };

            Mock<ITicketRepository> mockTicketRepository = new Mock<ITicketRepository>();
            mockTicketRepository.Setup(r => r.GetTicket(It.IsAny<Guid>())).ReturnsAsync(ticket);
            var sut = new StatusService(mockTicketRepository.Object);

            //Act
            var response = await sut.GetTicketResult(Guid.NewGuid());

            //Assert
            Assert.Equal(expectedResult, response.LineStatus[0].Result);
        }

        [Fact]
        public async void StatusService_GetTicketResult_NumberAllSame_ReturnsFive()
        {
            //Assign
            var expectedResult = 5;

            var ticket = new Ticket() { Id = Guid.NewGuid()};
            ticket.Lines = new List<Line>()
            {
                new Line() { Numbers = "1, 1, 1"}
            };

            Mock<ITicketRepository> mockTicketRepository = new Mock<ITicketRepository>();
            mockTicketRepository.Setup(r => r.GetTicket(It.IsAny<Guid>())).ReturnsAsync(ticket);
            var sut = new StatusService(mockTicketRepository.Object);

            //Act
            var response = await sut.GetTicketResult(Guid.NewGuid());

            //Assert
            Assert.Equal(expectedResult, response.LineStatus[0].Result);
        }

        [Fact]
        public async void StatusService_GetTicketResult_NumbersDiffFromFirst_ReturnsOne()
        {
            //Assign
            var expectedResult = 1;

            var ticket = new Ticket() { Id = Guid.NewGuid()};
            ticket.Lines = new List<Line>()
            {
                new Line() { Numbers = "1, 0, 2"}
            };

            Mock<ITicketRepository> mockTicketRepository = new Mock<ITicketRepository>();
            mockTicketRepository.Setup(r => r.GetTicket(It.IsAny<Guid>())).ReturnsAsync(ticket);
            var sut = new StatusService(mockTicketRepository.Object);

            //Act
            var response = await sut.GetTicketResult(Guid.NewGuid());

            //Assert
            Assert.Equal(expectedResult, response.LineStatus[0].Result);
        }

        [Fact]
        public async void StatusService_GetTicketResult_Default_ReturnsZero()
        {
            //Assign
            var expectedResult = 0;

            var ticket = new Ticket() { Id = Guid.NewGuid()};
            ticket.Lines = new List<Line>()
            {
                new Line() { Numbers = "0, 1, 0"}
            };

            Mock<ITicketRepository> mockTicketRepository = new Mock<ITicketRepository>();
            mockTicketRepository.Setup(r => r.GetTicket(It.IsAny<Guid>())).ReturnsAsync(ticket);
            var sut = new StatusService(mockTicketRepository.Object);

            //Act
            var response = await sut.GetTicketResult(Guid.NewGuid());

            //Assert
            Assert.Equal(expectedResult, response.LineStatus[0].Result);
        }

        [Fact]
        public async void StatusService_GetTicketResult_TicketDoesNotExist_ThrowsTicketNotFoundException()
        {
            //Assign
            Ticket nullTicket = null;
            Mock<ITicketRepository> mockTicketRepository = new Mock<ITicketRepository>();
            mockTicketRepository.Setup(r => r.GetTicket(It.IsAny<Guid>())).ReturnsAsync(nullTicket);
            var sut = new StatusService(mockTicketRepository.Object);

            //Act
            Func<Task> act = () => sut.GetTicketResult(Guid.NewGuid());

            //Assert
            var exception = await Assert.ThrowsAsync<TicketNotFoundException>(act);
            Assert.Equal("Ticket Does Not Exist", exception.Message);
        }
    }
}