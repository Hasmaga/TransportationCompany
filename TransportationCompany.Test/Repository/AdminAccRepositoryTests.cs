using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Moq;
using System.Security.Claims;
using TransportationCompany.Config;
using TransportationCompany.DbContexts;
using TransportationCompany.Model;
using TransportationCompany.Model.Dto;
using TransportationCompany.Repositories;
using Xunit;

namespace TransportationCompany.Test.Repository
{
    public class AdminAccRepositoryTests
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<AdminAccRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminAccRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyDatabase")
                .Options;
            _dbContext = new ApplicationDbContext(options);
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new MappingConfig())).CreateMapper();
            _logger = new Mock<ILogger<AdminAccRepository>>().Object;
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _httpContextAccessor = new Mock<IHttpContextAccessor>().Object;
        }

        //[Fact]
        //public async Task CreateNewAccountForTransportation_WithValidRole_ShouldCreateNewAccount()
        //{
        //    // Arrange            
        //    var adminAccRepository = new AdminAccRepository(_dbContext, _mapper, _logger, _configuration, _httpContextAccessor);
            
        //    var pas = new Passenger
        //    (
        //        name: "Admin",
        //        email: "Admin@example.com",
        //        phone: "0123456789",
        //        dob: null,
        //        createdDate: DateTime.Now,
        //        address: null,
        //        avatar: null
        //    );
        //    await _dbContext.Passengers.AddAsync(pas);
        //    await _dbContext.SaveChangesAsync();

        //    var password = "Admin1234@";            
        //    var passHash = new byte[64];
        //    var passSalt = new byte[128];
        //    using (var hmac = new System.Security.Cryptography.HMACSHA512())
        //    {
        //        passSalt = hmac.Key;
        //        passHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        //    }
        //    var pasLogin = new PassengerLogin
        //    (
        //        passengerId: pas.Id,
        //        passwordHash: Convert.ToBase64String(passHash),
        //        passwordSalt: Convert.ToBase64String(passSalt),
        //        status: true,
        //        authType: "AdminAcc"
        //    );
        //    await _dbContext.PassengerLogins.AddAsync(pasLogin);
        //    await _dbContext.SaveChangesAsync();

        //    _httpContextAccessor.Setup(x => x.HttpContext.User.FindFirstValue(ClaimTypes.Sid)).Returns(accountId);

        //    var newCom = new RegisterCompanyResDto
        //    {
        //        Name = "Test Company",
        //        Email = "testcompany@test.com",
        //        Phone = "1234567890",
        //        Address = "Test Address",
        //        Password = "testpassword"
        //    };
        //    var repository = new AdminAccRepository(_dbContext, _mapper, _logger, _configuration, httpContextAccessor);

        //    // Act
        //    var result = await repository.CreateNewAccountForTransportation(newCom);

        //    // Assert
        //    Assert.True(result);
        //}

        //[Fact]
        //public async Task CreateNewAccountForTransportation_WithInvalidRole_ShouldReturnFalse()
        //{
        //    // Arrange
        //    var httpContext = new DefaultHttpContext();
        //    httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        //    {
        //        new Claim(ClaimTypes.Name, "TestingUser"),
        //        new Claim(ClaimTypes.Role, "User")
        //    }, "test"));
        //    var httpContextAccessor = new HttpContextAccessor { HttpContext = httpContext };

        //    var newCom = new RegisterCompanyResDto
        //    {
        //        Name = "Test Company",
        //        Email = "testcompany@test.com",
        //        Phone = "1234567890",
        //        Address = "Test Address",
        //        Password = "testpassword"
        //    };
        //    var repository = new AdminAccRepository(_dbContext, _mapper, _logger, _configuration, httpContextAccessor);

        //    // Act
        //    var result = await repository.CreateNewAccountForTransportation(newCom);

        //    // Assert
        //    Assert.False(result);
        //}
    }
}
