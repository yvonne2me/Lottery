using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AutoMapper;
using Controllers;
using Logging;
using Exceptions;
using Models.API;
using Models.Domain;
using Services;
using Microsoft.AspNetCore.Mvc;

namespace ControllerTests
{
    public class TicketControllerTests
    {
        Mock<IFileLogger> mockLogger;
        Mock<IMapper> mockMapper;
        Mock<ITicketService> mockTicketService;
        TicketRequest ticketRequest;

        [Fact]
        public async void TicketController_Post_NullRequest_Returns_BadRequest()
        {
            //Assign
            SetupTestInfo();
            var sut = new TicketController(mockLogger.Object, mockMapper.Object, mockTicketService.Object);
            TicketRequest ticketRequest = null;

            //Act
            var response = await sut.Post(ticketRequest);
            var badResponse = response as BadRequestObjectResult;

            //Assert
            Assert.Equal(400, badResponse.StatusCode);
            Assert.Equal("No Ticket information provided", badResponse.Value);
        }

        [Fact]
        public async void TicketController_Post_ValidRequest_Returns_OK()
        {
            //Assign
            SetupTestInfo();
            var sut = new TicketController(mockLogger.Object, mockMapper.Object, mockTicketService.Object);

            //Act
            var response = await sut.Post(ticketRequest);
            var okResponse = response as OkObjectResult;

            //Assert
            Assert.Equal(200, okResponse.StatusCode);
        }

        [Fact]
        public async void TicketController_Post_ExceptionThrown_Returns_Error()
        {
            //Assign
            SetupTestInfo();
            this.mockTicketService.Setup(s => s.CreateTicket(It.IsAny<TicketRequest>())).Throws(new Exception());
            var sut = new TicketController(mockLogger.Object, mockMapper.Object, mockTicketService.Object);

            //Act
            Func<Task> act = () => sut.Post(ticketRequest);

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Error occurred while saving Ticket", exception.Message);
        }

        [Fact]
        public async void TicketController_Put_NullRequest_Returns_BadRequest()
        {
            //Assign
            SetupTestInfo();
            var sut = new TicketController(mockLogger.Object, mockMapper.Object, mockTicketService.Object);
            TicketRequest ticketRequest = null;
            Guid id = Guid.NewGuid();

            //Act
            var response = await sut.Put(id, ticketRequest);
            var badResponse = response as BadRequestObjectResult;

            //Assert
            Assert.Equal(400, badResponse.StatusCode);
            Assert.Equal("No Ticket information provided", badResponse.Value);
        }

        [Fact]
        public async void TicketController_Put_ValidRequest_Returns_OK()
        {
            //Assign
            SetupTestInfo();
            var sut = new TicketController(mockLogger.Object, mockMapper.Object, mockTicketService.Object);

            //Act
            var response = await sut.Put(Guid.NewGuid(), ticketRequest);
            var okResponse = response as OkObjectResult;

            //Assert
            Assert.Equal(200, okResponse.StatusCode);
        }

        [Fact]
        public async void TicketController_Put_ExceptionThrown_Returns_Error()
        {
            //Assign
            SetupTestInfo();
            this.mockTicketService.Setup(s => s.UpdateTicket(It.IsAny<Guid>(), It.IsAny<TicketRequest>())).Throws(new Exception());
            var sut = new TicketController(mockLogger.Object, mockMapper.Object, mockTicketService.Object);

            //Act
            Func<Task> act = () => sut.Put(Guid.NewGuid(), ticketRequest);

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Error occurred while updating Ticket", exception.Message);
        }

        [Fact]
        public async void TicketController_GetTicket_NotFound_Returns_NotFound()
        {
            //Assign
            SetupTestInfo();
            this.mockTicketService.Setup(s => s.GetTicket(It.IsAny<Guid>())).Throws(new TicketNotFoundException("Ticket Does Not Exist"));
            var sut = new TicketController(mockLogger.Object, mockMapper.Object, mockTicketService.Object);

            //Act
            var response = await sut.Get(Guid.NewGuid());
            
            //Assert
            var notFoundResponse = Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal(404, notFoundResponse.StatusCode);
        }

        [Fact]
        public async void TicketController_GetTicket_Found_Returns_Ticket()
        {
            //Assign
            SetupTestInfo();

            var newTicket = CreateTicket();

            this.mockTicketService.Setup(s => s.GetTicket(It.IsAny<Guid>())).ReturnsAsync(newTicket);
            var sut = new TicketController(mockLogger.Object, mockMapper.Object, mockTicketService.Object);

            //Act
            var response = await sut.Get(Guid.NewGuid());
            var okResult = response as OkObjectResult;

            //Assert
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async void TicketController_GetAllTickets_NotFound_Returns_NotFound()
        {
            //Assign
            SetupTestInfo();
            this.mockTicketService.Setup(s => s.GetAllTickets()).Throws(new TicketNotFoundException("Ticket Does Not Exist"));
            var sut = new TicketController(mockLogger.Object, mockMapper.Object, mockTicketService.Object);

            //Act
            var response = await sut.Get();
            
            //Assert
            var notFoundResponse = Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal(404, notFoundResponse.StatusCode);
        }

        [Fact]
        public async void TicketController_GetAllTickets_Found_Returns_Ticket()
        {
            //Assign
            SetupTestInfo();

            List<Ticket> listOfTickets = new List<Ticket>();

            for(var i=0; i<4; i++)
            {
                var ticket = CreateTicket();
                listOfTickets.Add(ticket);
            }

            this.mockTicketService.Setup(s => s.GetAllTickets()).ReturnsAsync(listOfTickets);
            var sut = new TicketController(mockLogger.Object, mockMapper.Object, mockTicketService.Object);

            //Act
            var response = await sut.Get();
            var okResult = response as OkObjectResult;
            var result = okResult.Value as List<TicketResponse>;

            //Assert
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(4, result.Count);
        }

        private void SetupTestInfo()
        {
            this.mockLogger = new Mock<IFileLogger>();            
            this.mockMapper = new Mock<IMapper>();
            this.mockTicketService = new Mock<ITicketService>();
            this.ticketRequest = new TicketRequest()
            {
                NumberOfLines = 3
            };
        }
        
        private Ticket CreateTicket()
        {
            Guid ticketId = Guid.NewGuid();
            
            Line line = new Line();
            line.Id = Guid.NewGuid();
            line.TicketId = ticketId;
            line.Numbers = "2, 1, 0";

            List<Line> lines = new List<Line>(){};

            lines.Add(line);

            Ticket newTicket = new Ticket()
            {
                Id = ticketId,
                Lines = lines
            };

            return newTicket;
        }
    }
}