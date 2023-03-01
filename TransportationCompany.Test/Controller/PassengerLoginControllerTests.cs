using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using TransportationCompany.Controllers;
using TransportationCompany.DbContexts;
using TransportationCompany.Enum;
using TransportationCompany.Model;
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
        public async Task Test_LoginAsync_ValidEmailAndPassword_ReturnsOk()
        {
            // Arrange
            string email = "test@example.com";
            string password = "password123";
            string token = "token123";
            _mockRepository.Setup(x => x.LoginWithEmailAsync(email, password)).ReturnsAsync(token);

            // mock is fake object to test the real object (controller) without the real object (repository) 

            // Act
            var result = await _controller.LoginAsync(email, null, password);

            // Assert
            Assert.NotNull(result);
            var okObjectResult = result.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var response = okObjectResult.Value as CommonResDto;
            Assert.NotNull(response);
            Assert.True(response.IsSuccess);            
        }

        [Fact]
        public async Task Test_LoginAsync_InvalidEmail_ReturnsBadRequest()
        {
            // Arrange
            string email = "test@example";
            string password = "password123";

            // Act
            var result = await _controller.LoginAsync(email, null, password);

            // Assert
            Assert.NotNull(result);
            var badRequestObjectResult = result.Result as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);
            var response = badRequestObjectResult.Value as CommonResDto;
            Assert.NotNull(response);
            Assert.False(response.IsSuccess);            
        }        
    }
}