using Xunit;
using Moq;
using AutoMapper;
using Controllers;
using Logging;
using Models.API;
using Mappers;
using Microsoft.AspNetCore.Mvc;

namespace ControllerTests
{
    public class TicketControllerTests
    {
        Mock<IFileLogger> mockLogger;
        Mock<IMapper> mockMapper;
        TicketRequest ticketRequest;

        [Fact]
        public async void TicketController_Post_NullRequest_Returns_BadRequest()
        {
            //Assign
            SetupTestInfo();
            var sut = new TicketController(mockLogger.Object, mockMapper.Object);
            TicketRequest ticketRequest = null;

            //Act
            var response = await sut.Post(ticketRequest);
            var badResponse = response as BadRequestObjectResult;

            //Assert
            Assert.Equal(400, badResponse.StatusCode);
            Assert.Equal("No Ticket information provided", badResponse.Value);
        }

        private void SetupTestInfo()
        {
            this.mockLogger = new Mock<IFileLogger>();            
            this.mockMapper = new Mock<IMapper>();
            this.ticketRequest = new TicketRequest()
            {
                NumberOfLines = 10
            };
        }
    }
}