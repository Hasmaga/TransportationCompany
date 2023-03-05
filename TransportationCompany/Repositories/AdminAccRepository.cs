using AutoMapper;
using System.Security.Claims;
using TransportationCompany.DbContexts;
using TransportationCompany.Enum;
using TransportationCompany.Model;
using TransportationCompany.Model.Dto;

namespace TransportationCompany.Repositories
{
    public class AdminAccRepository : IAdminAccRepository
    {
        private readonly ApplicationDbContext _db;
        private IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AdminAccRepository(ApplicationDbContext db, IMapper mapper, ILogger<AdminAccRepository> logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        private void CreatePassHash(String pass, out byte[] passHash, out byte[] passSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passSalt = hmac.Key;
                passHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));
            }
        }

        private Task<string> GetAccountRole()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            }
            return Task.FromResult(result);
        }

        public async Task<bool> CreateNewAccountForTransportation(RegisterCompanyResDto newCom)
        {
            _logger.LogInformation("Create New Account For Transportation");
            string role = await GetAccountRole();
            if (role != "AdminAcc")
            {
                return false;
                throw new Exception(ErrorCode.NOT_AUTHORIZED);
            }
            try
            {
                Passenger newCompany = new Passenger(newCom.Name, newCom.Email, newCom.Phone, null, DateTime.Now, newCom.Address, null);
                await _db.Passengers.AddAsync(newCompany);
                await _db.SaveChangesAsync();

                CreatePassHash(newCom.Password, out byte[] passHash, out byte[] passSalt);

                PassengerLogin newAccTran = new PassengerLogin(newCompany.Id, true, Convert.ToBase64String(passHash), Convert.ToBase64String(passSalt), "AdminCompany");
                await _db.PassengerLogins.AddAsync(newAccTran);
                await _db.SaveChangesAsync();

                Company newCompanyInfo = new Company(newCom.Name, newCom.Address, newCom.Email, newCom.Phone);
                await _db.Companies.AddAsync(newCompanyInfo);
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ErrorCode.ADD_ACC_ERROR);
            }
        }
        
    }
}
