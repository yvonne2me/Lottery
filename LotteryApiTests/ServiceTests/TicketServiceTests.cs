using Xunit;
using Moq;
using Models.API;
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
            var ticket = CreateTicket();
            var lines = CreateLines(ticket.Id);

            TicketRequest ticketRequest = new TicketRequest()
            {
                NumberOfLines = 6
            };

            Mock<ILineService> mockLineService = new Mock<ILineService>();
            Mock<ITicketRepository> mockTicketRepository = new Mock<ITicketRepository>();
            mockLineService.Setup(l => l.CreateLines(It.IsAny<Guid>(), It.IsAny<int>())).Returns(lines);
            mockTicketRepository.Setup(r => r.SaveTicket(It.IsAny<Ticket>())).ReturnsAsync(ticket);
            var sut = new TicketService(mockLineService.Object, mockTicketRepository.Object);

            //Act
            var ticketResponse = await sut.CreateTicket(ticketRequest);

            //Assert
            Assert.NotEqual(Guid.Empty, ticketResponse.Id);
        }

        [Fact]
        public async void TicketService_UpdateTicket_ReturnsTicket()
        {
            //Assign
            var ticket = CreateTicket();
            var lines = CreateLines(ticket.Id);
            var ticketRequest = new TicketRequest()
            {
                NumberOfLines = 4
            };

            Mock<ILineService> mockLineService = new Mock<ILineService>();
            Mock<ITicketRepository> mockTicketRepository = new Mock<ITicketRepository>();
            mockLineService.Setup(l => l.CreateLines(It.IsAny<Guid>(), It.IsAny<int>())).Returns(lines);
            mockTicketRepository.Setup(r => r.GetTicket(It.IsAny<Guid>())).ReturnsAsync(ticket);
            mockTicketRepository.Setup(r => r.UpdateTicket(It.IsAny<Ticket>(), It.IsAny<int>())).ReturnsAsync(ticket);
            var sut = new TicketService(mockLineService.Object, mockTicketRepository.Object);

            //Act
            var ticketResponse = await sut.UpdateTicket(ticket.Id, ticketRequest);

            //Assert
            Assert.NotNull(ticketResponse);
        }

        [Fact]
        public async void TicketService_GetTicket_ReturnsTicket()
        {
            //Assign
            var ticket = CreateTicket();
            var lines = CreateLines(ticket.Id);

            Mock<ILineService> mockLineService = new Mock<ILineService>();
            Mock<ITicketRepository> mockTicketRepository = new Mock<ITicketRepository>();
            mockLineService.Setup(l => l.CreateLines(It.IsAny<Guid>(), It.IsAny<int>())).Returns(lines);
            mockTicketRepository.Setup(r => r.GetTicket(It.IsAny<Guid>())).ReturnsAsync(ticket);
            var sut = new TicketService(mockLineService.Object, mockTicketRepository.Object);

            //Act
            var ticketResponse = await sut.GetTicket(ticket.Id);

            //Assert
            Assert.Equal(ticket.Id, ticketResponse.Id);
        }

        [Fact]
        public async void TicketService_GetAllTickets_ReturnsTickets()
        {
            //Assign
            List<Ticket> listOfTickets = new List<Ticket>();
            var lines = CreateLines(Guid.NewGuid());

            for(var i=0; i<4; i++)
            {
                var ticket = CreateTicket();
                listOfTickets.Add(ticket);
            }

            Mock<ILineService> mockLineService = new Mock<ILineService>();
            Mock<ITicketRepository> mockTicketRepository = new Mock<ITicketRepository>();
            mockLineService.Setup(l => l.CreateLines(It.IsAny<Guid>(), It.IsAny<int>())).Returns(lines);
            mockTicketRepository.Setup(r => r.GetAllTickets()).ReturnsAsync(listOfTickets);
            var sut = new TicketService(mockLineService.Object, mockTicketRepository.Object);

            //Act
            var ticketResponse = await sut.GetAllTickets();

            //Assert
            Assert.Equal(4, ticketResponse.Count);
        }
        
        private Ticket CreateTicket()
        {
            Guid ticketId = Guid.NewGuid();

            Ticket newTicket = new Ticket()
            {
                Id = Guid.NewGuid(),
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

            List<Line> lines = new List<Line>(){};

            lines.Add(line);

            return lines;
        }
    }
}