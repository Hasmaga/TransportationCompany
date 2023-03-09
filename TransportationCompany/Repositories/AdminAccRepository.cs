using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        
        public async Task<PassengerLogin> GetAccountLogin()
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                throw new Exception(ErrorCode.NOT_AUTHORIZED);
            }
            var result = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid));
            var acc = await _db.PassengerLogins.FindAsync(result);
            if (acc == null)
            {
                throw new Exception(ErrorCode.NOT_AUTHORIZED);
            }
            return acc;
        }

        public async Task<bool> CreateNewAccountForTransportation(RegisterCompanyResDto newCom)
        {
            _logger.LogInformation("Create New Account For Transportation");
            var pas = await GetAccountLogin();
            if (pas.AuthType != "AdminAcc")
            {
                return false;                
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

        public async Task<List<AccountLoginResDto>> GetAllAccount()
        {
            _logger.LogInformation("Get All Account");
            var acc = await GetAccountLogin();
            if (acc.AuthType != "AdminAcc")
            {
                throw new UnauthorizedAccessException(ErrorCode.NOT_AUTHORIZED);
            }
            try
            {
                var query = (from r in _db.Passengers
                             join l in _db.PassengerLogins on r.Id equals l.PassengerId
                             select new AccountLoginResDto
                             {
                                 Id = l.Id,
                                 Name = r.Name,
                                 Email = r.Email,
                                 Phone = r.Phone,
                                 Status = l.Status, 
                                 CreatedDate = r.CreatedDate,
                                 Address = r.Address,
                                 Avatar = r.Avatar,
                                 AuthType = l.AuthType
                             }).ToListAsync();
                return await query;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ErrorCode.GET_ALL_ACC_ERROR);
            }
        }
        
        public async Task<bool> ChangeStatusAccount(Guid Id)
        {
            _logger.LogInformation("Change Status Account");
            var acc = await GetAccountLogin();
            if (acc.AuthType != "AdminAcc")
                throw new UnauthorizedAccessException(ErrorCode.NOT_AUTHORIZED);
            try
            {
                var Account = await _db.PassengerLogins.FirstOrDefaultAsync(p => p.Id == Id);
                if (Account.Status == true)
                    Account.Status = false;
                else
                    Account.Status = true;
                _db.PassengerLogins.Update(Account);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ErrorCode.CHANGE_STATUS_ACC_ERROR);
            }
        }
        
    }
}
