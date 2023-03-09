using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using TransportationCompany.Config;
using TransportationCompany.DbContexts;
using TransportationCompany.Model;
using TransportationCompany.Model.Dto;
using TransportationCompany.Repositories;
using Xunit;

namespace TransportationCompany.Test.Repository
{
    public class PassengerLoginRepositoryTests
    {
        // Here we testing repository mean testing the logic of repository in this project we will test the data can be CRUD
        // Line 16 to 33 : Create Fake database to test logic of repository 
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<PassengerLoginRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PassengerLoginRepositoryTests()
        {
            // Initialize dependencies
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyDatabase")
                .Options;
            _dbContext = new ApplicationDbContext(options);
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new MappingConfig())).CreateMapper();
            _logger = new Mock<ILogger<PassengerLoginRepository>>().Object;
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _httpContextAccessor = new Mock<IHttpContextAccessor>().Object;
        }

        // This will test LoginWithEmailAsync Repository with Email and Password and if it return token then the test is pass

        [Fact]
        public async Task LoginWithEmailAsync_WhenCalledWithValidCredentials_ReturnsToken()
        {
            // Arrange       
            // Create Fake Data for database to test
            var passenger = new Passenger
            (
                name: "John",
                email: "john@example.com",
                phone: "1234567890",
                dob: null,
                createdDate: DateTime.Now,
                address: null,
                avatar: null
            );
            var password = "password";
            // Password need to be hashed before save to database
            var passHash = new byte[64];
            var passSalt = new byte[128];
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passSalt = hmac.Key;
                passHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            // Save Fake Passenger to database
            _dbContext.Passengers.Add(passenger);
            await _dbContext.SaveChangesAsync();

            // Crete Fake PassengerLogin to database
            var passengerLogin = new PassengerLogin
            (
                passengerId: passenger.Id,
                passwordHash: Convert.ToBase64String(passHash),
                passwordSalt: Convert.ToBase64String(passSalt),
                status: true,
                authType: "User"
            );
            // Save Fake PassengerLogin to database
            _dbContext.PassengerLogins.Add(passengerLogin);
            await _dbContext.SaveChangesAsync();

            // This will config the repository to test fake database
            var repository = new PassengerLoginRepository(_dbContext, _mapper, _logger, _configuration, _httpContextAccessor);

            // Act
            // This call repository LoginWithEmailAsync to test email and password
            var token = await repository.LoginWithEmailAsync("john@example.com", "password");

            // Assert
            // If the token is return this test will pass
            Assert.NotNull(token);
        }

        // Ngược lại phía trên

        [Fact]
        public async Task LoginWithPhoneAsync_WhenCalledWithValidCredentials_ReturnsToken()
        {
            // Arrange
            var passenger = new Passenger
            (
                name: "John1",
                email: "john1@example.com",
                phone: "1235567890",
                dob: null,
                createdDate: DateTime.Now,
                address: null,
                avatar: null
            );
            var password = "password";
            var passHash = new byte[64];
            var passSalt = new byte[128];
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passSalt = hmac.Key;
                passHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            _dbContext.Passengers.Add(passenger);
            await _dbContext.SaveChangesAsync();
            var passengerLogin = new PassengerLogin
            (
                passengerId: passenger.Id,
                passwordHash: Convert.ToBase64String(passHash),
                passwordSalt: Convert.ToBase64String(passSalt),
                status: true,
                authType: "User"
            );
            _dbContext.PassengerLogins.Add(passengerLogin);
            await _dbContext.SaveChangesAsync();
            var repository = new PassengerLoginRepository(_dbContext, _mapper, _logger, _configuration, _httpContextAccessor);

            // Act
            var token = await repository.LoginWithPhoneAsync(passenger.Phone, password);

            // Assert
            Assert.NotNull(token);
        }

        [Fact]
        public async Task LoginWithPhoneAsync_WhenCalledWithInvalidCredentials_ReturnsBadRequest()
        {
            // Arrange
            var passenger = new Passenger
            (
                name: "John2",
                email: "john2@example.com",
                phone: "1234667890",
                dob: null,
                createdDate: DateTime.Now,
                address: null,
                avatar: null
            );
            var password = "password";
            var passHash = new byte[64];
            var passSalt = new byte[128];
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passSalt = hmac.Key;
                passHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            _dbContext.Passengers.Add(passenger);
            await _dbContext.SaveChangesAsync();
            var passengerLogin = new PassengerLogin
            (
                passengerId: passenger.Id,
                passwordHash: Convert.ToBase64String(passHash),
                passwordSalt: Convert.ToBase64String(passSalt),
                status: true,
                authType: "User"
            );
            _dbContext.PassengerLogins.Add(passengerLogin);
            await _dbContext.SaveChangesAsync();
            var repository = new PassengerLoginRepository(_dbContext, _mapper, _logger, _configuration, _httpContextAccessor);

            // Act
            try
            {
                var token = await repository.LoginWithPhoneAsync("0913412423", password);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.Equal("THIS_ACCOUNT_NOT_FOUND", ex.Message);
            }
        }

        [Fact]
        public async Task RegistrationAccountAsync_CheckIfPassengerIsAllReadyExist_ThenCreateNewPassengerAccount_ReturnTrue()
        {
            // Arrange
            var passenger = new Passenger
            (
                name: "John3",
                email: "John3@gmail.com",
                phone: "1235577890",
                dob: null,
                createdDate: DateTime.Now,
                address: null,
                avatar: null
            );
            var password = "John1234@";

            _dbContext.Passengers.Add(passenger);
            await _dbContext.SaveChangesAsync();

            RegistrationAccountResDto newLoginPassenger = new RegistrationAccountResDto
            {
                Name = passenger.Name,
                Email = passenger.Email,
                Phone = passenger.Phone,
                Password = password
            };

            var repository = new PassengerLoginRepository(_dbContext, _mapper, _logger, _configuration, _httpContextAccessor);

            // Act
            var result = await repository.RegistrationAccountAsync(newLoginPassenger);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task RegistrationAccountAsync_CheckIfPassengerIsNotExist_ThenCreateNewPassengerAccount_ReturnTrue()
        {
            var passenger = new Passenger
            (
                name: "John4",
                email: "John4@gmail.com",
                phone: "1224567880",
                dob: null,
                createdDate: DateTime.Now,
                address: null,
                avatar: null
            );
            var password = "John1234@";

            RegistrationAccountResDto newLoginPassenger = new RegistrationAccountResDto
            {
                Name = passenger.Name,
                Email = passenger.Email,
                Phone = passenger.Phone,
                Password = password
            };

            var repository = new PassengerLoginRepository(_dbContext, _mapper, _logger, _configuration, _httpContextAccessor);

            // Act
            var result = await repository.RegistrationAccountAsync(newLoginPassenger);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task RegistrationAccountAsync_IfPassengerIsAlreadyRegister_ReturnFalse()
        {
            // Arrange
            var passenger = new Passenger
            (
                name: "John",
                email: "John@gmail.com",
                phone: "1234567890",
                dob: null,
                createdDate: DateTime.Now,
                address: null,
                avatar: null
            );
            var password = "John1234@";
            var passHash = new byte[64];
            var passSalt = new byte[128];
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passSalt = hmac.Key;
                passHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            _dbContext.Passengers.Add(passenger);
            await _dbContext.SaveChangesAsync();
            var passengerLogin = new PassengerLogin
            (
                passengerId: passenger.Id,
                passwordHash: Convert.ToBase64String(passHash),
                passwordSalt: Convert.ToBase64String(passSalt),
                status: true,
                authType: "User"
            );
            _dbContext.PassengerLogins.Add(passengerLogin);
            await _dbContext.SaveChangesAsync();

            RegistrationAccountResDto newLoginPassenger = new RegistrationAccountResDto
            {
                Name = passenger.Name,
                Email = passenger.Email,
                Phone = passenger.Phone,
                Password = password
            };

            var repository = new PassengerLoginRepository(_dbContext, _mapper, _logger, _configuration, _httpContextAccessor);

            // Act
            var result = await repository.RegistrationAccountAsync(newLoginPassenger);

            // Assert
            Assert.False(result);
        }
    }
}
