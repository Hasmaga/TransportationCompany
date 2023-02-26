using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TransportationCompany.Controllers;
using TransportationCompany.Enum;
using TransportationCompany.Model.Dto;
using TransportationCompany.Repositories;
using Xunit;

namespace TransportationCompany.Test.Controller
{
    public class PassengerLoginControllerTests
    {
        private readonly Mock<IPassengerLoginRepository> _mockRepository;
        private readonly Mock<ILogger<PassengerLoginController>> _mockLogger;
        private readonly PassengerLoginController _controller;

        public PassengerLoginControllerTests()
        {
            _mockRepository = new Mock<IPassengerLoginRepository>();
            _mockLogger = new Mock<ILogger<PassengerLoginController>>();
            _controller = new PassengerLoginController(_mockRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task LoginAsync_ReturnsOkObjectResult_WhenEmailAndPasswordAreValid()
        {
            // Arrange
            var email = "chilythuongtin122@gmail.com";
            var password = "Ankhang28";
            var token = "valid_token";
            _mockRepository.Setup(x => x.LoginWithEmailAsync(email, password)).ReturnsAsync(token);

            // Act
            var result = await _controller.LoginAsync(email, null, password);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var commonResDto = Assert.IsType<CommonResDto>(okResult.Value);
            Assert.True(commonResDto.IsSuccess);
            Assert.Equal(token, commonResDto.Result);
            Assert.Equal("Passenger Login Successfully", commonResDto.DisplayMessage);
        }        
    }
}