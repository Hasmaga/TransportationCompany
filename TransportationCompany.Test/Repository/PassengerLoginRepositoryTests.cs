using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using TransportationCompany.Config;
using TransportationCompany.DbContexts;
using TransportationCompany.Model;
using TransportationCompany.Repositories;

public class PassengerLoginRepositoryTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<PassengerRepository> _logger;
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
        _logger = new Mock<ILogger<PassengerRepository>>().Object;
        _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        _httpContextAccessor = new Mock<IHttpContextAccessor>().Object;
    }

    [Fact]
    public async Task LoginWithEmailAsync_WhenCalledWithValidCredentials_ReturnsToken()
    {
        // Arrange       
        var passenger = new Passenger
        (            
            name: "John",
            email: "john@example.com",
            phone: "1234567890"
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
            status: true
        );
        _dbContext.PassengerLogins.Add(passengerLogin);
        await _dbContext.SaveChangesAsync();
        var repository = new PassengerLoginRepository(_dbContext, _mapper, _logger, _configuration, _httpContextAccessor);
        
        // Act
        var token = await repository.LoginWithEmailAsync("john@example.com", "password");
        
        // Assert
        Assert.NotNull(token);
    }

    [Fact]
    public async Task LoginWithPhoneAsync_WhenCalledWithValidCredentials_ReturnsToken()
    {
        // Arrange
        var passenger = new Passenger
        (
            name: "John",
            email: "john@example.com",
            phone: "1234567890"
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
            status: true
        );        
        _dbContext.PassengerLogins.Add(passengerLogin);
        await _dbContext.SaveChangesAsync();
        var repository = new PassengerLoginRepository(_dbContext, _mapper, _logger, _configuration, _httpContextAccessor);

        // Act
        var token = await repository.LoginWithPhoneAsync(passenger.Phone, password);

        // Assert
        Assert.NotNull(token);
    }
}