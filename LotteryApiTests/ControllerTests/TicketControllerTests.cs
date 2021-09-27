using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AutoMapper;
using Controllers;
using Logging;
using Models.API;
using Models.Domain;
using Mappers;
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
            this.mockTicketService.Setup(s => s.CreateTicket(It.IsAny<Ticket>())).Throws(new Exception());
            var sut = new TicketController(mockLogger.Object, mockMapper.Object, mockTicketService.Object);

            //Act
            Func<Task> act = () => sut.Post(ticketRequest);

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Error occurred while saving Ticket", exception.Message);
        }

        [Fact]
        public async void TicketController_GetTicket_NotFound_Returns_NotFound()
        {
            //Assign
            SetupTestInfo();
            Ticket ticket = null;
            this.mockTicketService.Setup(s => s.GetTicket(It.IsAny<Guid>())).ReturnsAsync(ticket);
            var sut = new TicketController(mockLogger.Object, mockMapper.Object, mockTicketService.Object);

            //Act
            var response = await sut.Get(Guid.NewGuid());
            
            //Assert
            var notFoundResponse = Assert.IsType<NotFoundResult>(response);
            Assert.Equal(404, notFoundResponse.StatusCode);
        }

        [Fact]
        public async void TicketController_GetTicket_Found_Returns_Ticket()
        {
            //Assign
            SetupTestInfo();
            var expectedNumberOfLines = 4;
            Ticket newTicket = new Ticket()
            {
                Id = Guid.NewGuid(),
                NumberOfLines = expectedNumberOfLines
            };

            this.mockTicketService.Setup(s => s.GetTicket(It.IsAny<Guid>())).ReturnsAsync(newTicket);
            var sut = new TicketController(mockLogger.Object, mockMapper.Object, mockTicketService.Object);

            //Act
            var response = await sut.Get(Guid.NewGuid());
            var okResult = response as OkObjectResult;
            var result = okResult.Value as Ticket;

            //Assert
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(expectedNumberOfLines, result.NumberOfLines);
        }

        [Fact]
        public async void TicketController_GetAllTickets_NotFound_Returns_NotFound()
        {
            //Assign
            SetupTestInfo();
            List<Ticket> listOfTickets = null;
            this.mockTicketService.Setup(s => s.GetAllTickets()).ReturnsAsync(listOfTickets);
            var sut = new TicketController(mockLogger.Object, mockMapper.Object, mockTicketService.Object);

            //Act
            var response = await sut.Get();
            
            //Assert
            var notFoundResponse = Assert.IsType<NotFoundResult>(response);
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
                var ticket = new Ticket()
                {
                    NumberOfLines = i
                };

                listOfTickets.Add(ticket);
            }

            this.mockTicketService.Setup(s => s.GetAllTickets()).ReturnsAsync(listOfTickets);
            var sut = new TicketController(mockLogger.Object, mockMapper.Object, mockTicketService.Object);

            //Act
            var response = await sut.Get();
            var okResult = response as OkObjectResult;
            var result = okResult.Value as List<Ticket>;

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
                NumberOfLines = 10
            };
        }
    }
}