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
        // Mock mean fake object that we can use to test our code
        // Line 24 to Line 26 : Create Mock object for each class that we want to test
        private readonly Mock<IPassengerLoginRepository> _mockRepository;
        private readonly Mock<ILogger<PassengerLoginController>> _mockLogger;
        private readonly PassengerLoginController _controller;

        // Line 29 to Line 34 : Create contructer object for each class that we want to test
        public PassengerLoginControllerTests()
        {
            _mockRepository = new Mock<IPassengerLoginRepository>();
            _mockLogger = new Mock<ILogger<PassengerLoginController>>();
            _controller = new PassengerLoginController(_mockRepository.Object, _mockLogger.Object);
        }

        // Testing controller mean testing the logic of controller in this project we will test validation of input
        
        // This will test Task LoginAsync Controller with Email and Number
        [Fact]
        public async Task Test_LoginAsync_ValidEmailAndPassword_ReturnsOk()
        {
            // Arrange
            // Create Fake object for input
            string email = "test@example.com";
            string password = "password123";
            string token = "token123";
            _mockRepository.Setup(x => x.LoginWithEmailAsync(email, password)).ReturnsAsync(token);

            // Act
            // Call LoginAsync Controller to test Fake object input
            var result = await _controller.LoginAsync(email, null, password);

            // Assert
            // Check result of LoginAsync Controller is Ok
            Assert.NotNull(result);
            // Check 200 or something else, if 200 the result is Ok
            var okObjectResult = result.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            // Check response of LoginAsync Controller return token or not 
            var response = okObjectResult.Value as CommonResDto;
            Assert.NotNull(response);
            // Check the object response.IsSuccess is true or not
            Assert.True(response.IsSuccess);            
        }

        [Fact]
        public async Task Test_LoginAsync_InvalidEmail_ReturnsBadRequest()
        {
            // Arrange
            // Create Fake object for input
            string email = "test@example";
            string password = "password123";

            // Act
            // Call LoginAsync Controller to test Fake object input
            var result = await _controller.LoginAsync(email, null, password);

            // Assert
            // Check result is null or not
            Assert.NotNull(result);
            // Check 400 or something else, if 400 the result is BadRequest then is True
            var badRequestObjectResult = result.Result as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);
            // Check response of LoginAsync return object CommonResDto.IsSuccess is false or not
            var response = badRequestObjectResult.Value as CommonResDto;
            Assert.NotNull(response);
            Assert.False(response.IsSuccess);
        }
    }
}