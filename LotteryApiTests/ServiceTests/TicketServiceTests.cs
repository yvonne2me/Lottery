using Xunit;
using Moq;
using Models.API;
using Models.Domain;
using Repositories;
using Services;
using System.Collections.Generic;
using System;
using Builders;

namespace ServiceTests
{
    public class TicketServiceTests
    {
        Mock<ITicketRepository> mockTicketRepository;
        Ticket ticket;
        
        [Fact]
        public async void TicketService_CreateTicket_AssignsId()
        {
            //Assign
            SetupMocksAndTestData();
            TicketRequest ticketRequest = new TicketRequest()
            {
                NumberOfLines = 6
            };           
            var sut = new TicketService(this.mockTicketRepository.Object);

            //Act
            var ticketResponse = await sut.CreateTicket(ticketRequest);

            //Assert
            Assert.NotEqual(Guid.Empty, ticketResponse.Id);
        }

        [Fact]
        public async void TicketService_UpdateTicket_ReturnsTicket()
        {
            //Assign
            SetupMocksAndTestData();
            var ticketRequest = new TicketRequest()
            {
                NumberOfLines = 4
            };

            this.mockTicketRepository.Setup(r => r.UpdateTicket(It.IsAny<Ticket>(), It.IsAny<int>()))
                    .ReturnsAsync(this.ticket);
            var sut = new TicketService(this.mockTicketRepository.Object);

            //Act
            var ticketResponse = await sut.UpdateTicket(ticket.Id, ticketRequest);

            //Assert
            Assert.NotNull(ticketResponse);
        }

        [Fact]
        public async void TicketService_GetTicket_ReturnsTicket()
        {
            //Assign
            SetupMocksAndTestData();
            var sut = new TicketService(this.mockTicketRepository.Object);

            //Act
            var ticketResponse = await sut.GetTicket(ticket.Id);

            //Assert
            Assert.Equal(ticket.Id, ticketResponse.Id);
        }

        [Fact]
        public async void TicketService_GetAllTickets_ReturnsTickets()
        {
            //Assign
            SetupMocksAndTestData();

            List<Ticket> listOfTickets = new List<Ticket>();

            for(var i=0; i<4; i++)
            {
                var ticket = CreateTicket();
                listOfTickets.Add(ticket);
            }

            this.mockTicketRepository.Setup(r => r.GetAllTickets()).ReturnsAsync(listOfTickets);
            var sut = new TicketService(this.mockTicketRepository.Object);

            //Act
            var ticketResponse = await sut.GetAllTickets();

            //Assert
            Assert.Equal(4, ticketResponse.Count);
        }
        
        private void SetupMocksAndTestData()
        {
            this.ticket = CreateTicket();
            mockTicketRepository = new Mock<ITicketRepository>();
            mockTicketRepository.Setup(r => r.SaveTicket(It.IsAny<Ticket>())).ReturnsAsync(ticket);
            mockTicketRepository.Setup(r => r.GetTicket(It.IsAny<Guid>())).ReturnsAsync(ticket);
        }

        private Ticket CreateTicket()
        {
            Guid ticketId = Guid.NewGuid();
            Ticket newTicket = new Ticket(){ Id = ticketId };
            newTicket.Lines = LineBuilder.CreateLines(ticketId, 3);
            return newTicket;
        }

        // private List<Line> CreateLines(Guid ticketId)
        // {
        //     List<Line> lines = new List<Line>();
        //     Line line = new Line() { Id = Guid.NewGuid(), TicketId = ticketId};
        //     line.Numbers = "0, 1, 2";           
        //     lines.Add(line);

        //     return lines;
        // }
    }
}