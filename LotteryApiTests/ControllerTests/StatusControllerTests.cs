using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AutoMapper;
using Controllers;
using Logging;
using Models.API;
using Services;
using Microsoft.AspNetCore.Mvc;

namespace ControllerTests
{
    public class StatusControllerTests
    {
        Mock<IFileLogger> mockLogger;
        Mock<IMapper> mockMapper;
        Mock<IStatusService> mockStatusService;

        [Fact]
        public async void StatusController_Put_ValidRequest_Returns_OK()
        {
            //Assign
            SetupMocks();
            var sut = new StatusController(mockLogger.Object, mockMapper.Object, mockStatusService.Object);
            Status result = new Status();
            this.mockStatusService.Setup(s => s.GetTicketResult(It.IsAny<Guid>())).ReturnsAsync(result);

            Guid ticketId = Guid.NewGuid();

            //Act
            var response = await sut.Put(ticketId);
            var okResponse = response as OkObjectResult;

            //Assert
            Assert.Equal(200, okResponse.StatusCode);
        }

        [Fact]
        public async void StatusController_Put_ExceptionThrown_Returns_Error()
        {
            //Assign
            SetupMocks();
            this.mockStatusService.Setup(s => s.GetTicketResult(It.IsAny<Guid>())).Throws(new Exception());
            var sut = new StatusController(mockLogger.Object, mockMapper.Object, mockStatusService.Object);
            Guid ticketId = Guid.NewGuid();

            //Act
            Func<Task> act = () => sut.Put(ticketId);

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Error occurred while checking Status", exception.Message);
        }

        private void SetupMocks()
        {
            this.mockLogger = new Mock<IFileLogger>();            
            this.mockMapper = new Mock<IMapper>();
            this.mockStatusService = new Mock<IStatusService>();
        }
    }
}