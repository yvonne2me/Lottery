using Xunit;
using Moq;
using Models.API;
using Models.Domain;
using Repositories;
using Services;
using System.Collections.Generic;
using System;
using Builders;
using System.Threading.Tasks;
using Exceptions;

namespace ServiceTests
{
    public class TicketServiceTests
    {
        Mock<ITicketRepository> mockTicketRepository;
        Ticket ticket;
        const int NumberOfLines = 4;
        
        [Fact]
        public async void TicketService_CreateTicket_ReturnsTicket()
        {
            //Assign
            SetupMocksAndTestData();
            TicketRequest ticketRequest = new TicketRequest()
            {
                NumberOfLines = NumberOfLines
            };

            mockTicketRepository.Setup(r => r.SaveTicket(It.IsAny<Ticket>())).ReturnsAsync(this.ticket);
            var sut = new TicketService(this.mockTicketRepository.Object);

            //Act
            var ticketResponse = await sut.CreateTicket(ticketRequest);

            //Assert
            Assert.Equal(this.ticket.Id, ticketResponse.Id);
            Assert.Equal(NumberOfLines, ticketResponse.Lines.Count);
        }

        [Fact]
        public async void TicketService_CreateTicket_InvalidNumberOfLines_ThrowsArgumentException()
        {
            //Assign
            SetupMocksAndTestData();
            TicketRequest ticketRequest = new TicketRequest()
            {
                NumberOfLines = 0
            };

            mockTicketRepository.Setup(r => r.SaveTicket(It.IsAny<Ticket>())).ReturnsAsync(this.ticket);
            var sut = new TicketService(this.mockTicketRepository.Object);

            //Act
            Func<Task> act = () => sut.CreateTicket(ticketRequest);

            //Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(act);
            Assert.Equal("Invalid Number of Lines", exception.Message);
        }

        [Fact]
        public async void TicketService_UpdateTicket_ReturnsTicket()
        {
            //Assign
            SetupMocksAndTestData();
            TicketRequest ticketRequest = new TicketRequest()
            {
                NumberOfLines = NumberOfLines
            };

            this.mockTicketRepository.Setup(r => r.GetTicket(It.IsAny<Guid>())).ReturnsAsync(this.ticket);
            this.mockTicketRepository.Setup(r => r.UpdateTicket(It.IsAny<Ticket>(), It.IsAny<int>())).ReturnsAsync(this.ticket);
            var sut = new TicketService(this.mockTicketRepository.Object);

            //Act
            var ticketResponse = await sut.UpdateTicket(this.ticket.Id, ticketRequest);

            //Assert
            Assert.Equal(this.ticket.Id, ticketResponse.Id);
            Assert.Equal(NumberOfLines, ticketResponse.Lines.Count);
        }

        [Fact]
        public async void TicketService_UpdateTicket_TicketDoesNotExist_SaveTicketCalled()
        {
            //Assign
            SetupMocksAndTestData();
            TicketRequest ticketRequest = new TicketRequest()
            {
                NumberOfLines = NumberOfLines
            };

            Ticket nullTicket = null;
            this.mockTicketRepository.Setup(r => r.GetTicket(It.IsAny<Guid>())).ReturnsAsync(nullTicket);
            var sut = new TicketService(this.mockTicketRepository.Object);

            //Act
            var ticketResponse = await sut.UpdateTicket(this.ticket.Id, ticketRequest);
            
            //Assert
            this.mockTicketRepository.Verify(x => x.SaveTicket(It.IsAny<Ticket>()), Times.Once);
        }

        [Fact]
        public async void TicketService_UpdateTicket_AlreadyChecked_ThrowsArgumentException()
        {
            //Assign
            SetupMocksAndTestData();
            this.ticket.Checked = true;

            TicketRequest ticketRequest = new TicketRequest()
            {
                NumberOfLines = NumberOfLines
            };

            this.mockTicketRepository.Setup(r => r.GetTicket(It.IsAny<Guid>())).ReturnsAsync(this.ticket);
            var sut = new TicketService(this.mockTicketRepository.Object);

            //Act
            Func<Task> act = () => sut.UpdateTicket(this.ticket.Id, ticketRequest);

            //Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(act);
            Assert.Equal("Ticket already checked and cannot be updated", exception.Message);
        }

        [Fact]
        public async void TicketService_GetTicket_ReturnsTicket()
        {
            //Assign
            SetupMocksAndTestData();
            this.mockTicketRepository.Setup(r => r.GetTicket(It.IsAny<Guid>())).ReturnsAsync(this.ticket);
            var sut = new TicketService(this.mockTicketRepository.Object);

            //Act
            var ticketResponse = await sut.GetTicket(this.ticket.Id);

            //Assert
            Assert.Equal(this.ticket.Id, ticketResponse.Id);
        }

        [Fact]
        public async void TicketService_GetTicket_TicketDoesNotExist_ThrowsTicketNotFoundException()
        {
            //Assign
            SetupMocksAndTestData();
            Ticket nullTicket = null;
            this.mockTicketRepository.Setup(r => r.GetTicket(It.IsAny<Guid>())).ReturnsAsync(nullTicket);
            var sut = new TicketService(this.mockTicketRepository.Object);

            //Act
            Func<Task> act = () => sut.GetTicket(Guid.NewGuid());

            //Assert
            var exception = await Assert.ThrowsAsync<TicketNotFoundException>(act);
            Assert.Equal("Ticket Does Not Exist", exception.Message);
        }

        [Fact]
        public async void TicketService_GetAllTickets_ReturnsTickets()
        {
            //Assign
            SetupMocksAndTestData();

            List<Ticket> listOfTickets = new List<Ticket>();
            var ticketId = Guid.NewGuid();
            var numberOfTickets = 4;

            for(var i=0; i<numberOfTickets; i++)
            {
                var ticket = new Ticket(){ Id = ticketId };
                ticket.Lines = LineBuilder.CreateLines(ticketId, NumberOfLines);
                listOfTickets.Add(ticket);
            }

            this.mockTicketRepository.Setup(r => r.GetAllTickets()).ReturnsAsync(listOfTickets);
            var sut = new TicketService(this.mockTicketRepository.Object);

            //Act
            var ticketResponse = await sut.GetAllTickets();

            //Assert
            Assert.Equal(numberOfTickets, ticketResponse.Count);
        }

        [Fact]
        public async void TicketService_GetAllTickets_NoTickets_ThrowsTicketNotFoundException()
        {
            //Assign
            SetupMocksAndTestData();
            List<Ticket> nullListTicket = null;
            this.mockTicketRepository.Setup(r => r.GetAllTickets()).ReturnsAsync(nullListTicket);
            var sut = new TicketService(this.mockTicketRepository.Object);

            //Act
            Func<Task> act = () => sut.GetAllTickets();

            //Assert
            var exception = await Assert.ThrowsAsync<TicketNotFoundException>(act);
            Assert.Equal("No Tickets Found", exception.Message);
        }
        
        private void SetupMocksAndTestData()
        {
            var ticketId = Guid.NewGuid();
            this.ticket = new Ticket() 
            { 
                Id = Guid.NewGuid(),
                Lines = LineBuilder.CreateLines(ticketId, NumberOfLines)
            };
            mockTicketRepository = new Mock<ITicketRepository>();
        }
    }
}