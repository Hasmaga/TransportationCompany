using AutoMapper;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Validations;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using TransportationCompany.DbContexts;
using TransportationCompany.Enum;
using TransportationCompany.Model;
using TransportationCompany.Model.Dto;

namespace TransportationCompany.Repositories
{
    public class PassengerLoginRepository : IPassengerLoginRepository
    {
        private readonly ApplicationDbContext _db;
        private IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public PassengerLoginRepository(ApplicationDbContext db, IMapper mapper, ILogger<PassengerLoginRepository> logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        // Create password hash
        private void CreatePassHash(String pass, out byte[] passHash, out byte[] passSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passSalt = hmac.Key;
                passHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));
            }
        }


        // Verify password hash

        private bool VerifyPassHash(String pass, String passHash, String passSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(Convert.FromBase64String(passSalt)))
            {
                var computedHash = Convert.ToBase64String(hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass)));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passHash[i]) throw new UnauthorizedAccessException(ErrorCode.PASSWORD_INCORRECT);
                }
                return true;
            }
        }

        // Generate token

        private string GenerateToken(Guid Id, string AuthType)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );
            var jwt = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        // Login with email and password and return token if success

        public async Task<string> LoginWithEmailAsync(string email, string password)
        {
            _logger.LogInformation("Login Passenger with Email");
             var  query = (from r in _db.Passengers
                         join c in _db.PassengerLogins on r.Id equals c.PassengerId
                         where r.Email == email
                         select new
                         {
                             r.Id,
                             r.Name,
                             r.Email,
                             r.Phone,
                             c.Status,
                             c.PasswordHash,                             
                             c.PasswordSalt,
                             c.AuthType
                         }).FirstOrDefault();
            if (query == null) throw new UnauthorizedAccessException(ErrorCode.ACCOUNT_NOT_FOUND);
            if (!VerifyPassHash(password, query.PasswordHash, query.PasswordSalt)) throw new UnauthorizedAccessException(ErrorCode.PASSWORD_INCORRECT);
            if (query.Status == false) throw new UnauthorizedAccessException(ErrorCode.PASSENGER_NOT_ACTIVE);
            string token = GenerateToken(query.Id, query.AuthType);
            return token;
        }

        // Login with phone and password and return token if success

        public async Task<string> LoginWithPhoneAsync(string Phone, string Password)
        {
            _logger.LogInformation("Login Passenger with Phone");
            var query = (from r in _db.Passengers
                         join c in _db.PassengerLogins on r.Id equals c.PassengerId
                         where r.Phone == Phone
                         select new
                         {
                             r.Id,
                             r.Name,
                             r.Email,
                             r.Phone,
                             c.Status,
                             c.PasswordHash,
                             c.PasswordSalt,
                             c.AuthType
                         }).FirstOrDefault();
            if (query == null) throw new UnauthorizedAccessException(ErrorCode.ACCOUNT_NOT_FOUND);
            if (!VerifyPassHash(Password, query.PasswordHash, query.PasswordSalt)) throw new UnauthorizedAccessException(ErrorCode.PASSWORD_INCORRECT);
            if (query.Status == false) throw new UnauthorizedAccessException(ErrorCode.PASSENGER_NOT_ACTIVE);
            string token = GenerateToken(query.Id, query.AuthType);
            return token;            
        }

        // Check if the account is exist or not by email and phone

        public async Task<Passenger> CheckPassengerExist(string Phone, string Email)
        {
            _logger.LogInformation("Check Account Exist");
            try
            {
                var resultEmail = await _db.Passengers.FirstOrDefaultAsync(x => x.Email == Email);
                var resultPhone = await _db.Passengers.FirstOrDefaultAsync(x => x.Phone == Phone);
                
                if (resultEmail != null)                
                    return resultEmail;
                
                if (resultPhone != null)
                    return resultPhone;

                return null;
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error To Check Passenger Exist");
                return null;
            }
        }

        public async Task<bool> CheckAccountExist(string email, string phone)
        {
            _logger.LogInformation("Check Account Exist");
            try 
            {
                var result = (from r in _db.Passengers
                              join c in _db.PassengerLogins on r.Id equals c.PassengerId
                              where r.Email == email || r.Phone == phone
                              select new
                              {
                                  r.Id,
                                  r.Name,
                                  r.Email,
                                  r.Phone,
                                  c.Status,
                                  c.PasswordHash,
                                  c.PasswordSalt
                              }).FirstOrDefault();
                if (result == null) return false;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error To Check Account Exist");
                return false;
            }
        }       

        // Register new account with RegistrationAccountResDto 

        public async Task<bool> RegistrationAccountAsync(RegistrationAccountResDto passenger)
        {
            _logger.LogInformation("Registration Passenger");      
            if (await CheckAccountExist(passenger.Email, passenger.Phone))
                return false;
            try
            {
                var result = await CheckPassengerExist(passenger.Phone, passenger.Email);
                if (result != null)
                {
                    _logger.LogInformation("Create Account For Passenger");
                    CreatePassHash(passenger.Password, out byte[] passHash, out byte[] passSalt);
                    PassengerLogin newAccount = new PassengerLogin(result.Id, true, Convert.ToBase64String(passHash), Convert.ToBase64String(passSalt), "User");
                    await _db.PassengerLogins.AddAsync(newAccount);
                    await _db.SaveChangesAsync();
                    return true;
                }
                else if (result == null)
                {
                    _logger.LogInformation("Create Passenger Login");
                    Passenger newPas = new Passenger(passenger.Name, passenger.Email, passenger.Phone, null, DateTime.Now, null, null);
                    await _db.Passengers.AddAsync(newPas);
                    await _db.SaveChangesAsync();                    
                    
                    CreatePassHash(passenger.Password, out byte[] passHash, out byte[] passSalt);
                    PassengerLogin newAccount = new PassengerLogin(newPas.Id, true, Convert.ToBase64String(passHash), Convert.ToBase64String(passSalt), "User");                   
                    await _db.PassengerLogins.AddAsync(newAccount);
                    await _db.SaveChangesAsync();
                    
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error While Registration Passenger");
                return false;
            }            
        }

        private Task<Guid> GetGuidUser()
        {
            _logger.LogInformation("Get Guid User");
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid);
            return Task.FromResult(Guid.Parse(result));
        }
        
        public async Task<bool> UpdatePassengerAsync(PassengerInfoUpdateResDto passenger)
        {
            _logger.LogInformation("Update Passenger");
            var guidUser = GetGuidUser();
            if (guidUser == null)
            {
                throw new UnauthorizedAccessException(ErrorCode.NOT_AUTHORIZED);
            }
            try
            {
                var result = await _db.Passengers.FirstOrDefaultAsync(x => x.Id == guidUser.Result);
                if (result == null)
                    throw new Exception(ErrorCode.ACCOUNT_NOT_FOUND);
                
                if (passenger.Name != null)
                    result.Name = passenger.Name;
                if (passenger.Address != null)
                    result.Address = passenger.Address;
                if (passenger.Dob != null)
                    result.Dob = passenger.Dob;
                if (passenger.Email != null)
                    result.Email = passenger.Email;

                _db.Passengers.Update(result);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error While Update Passenger");
                return false;
            }
        }

        public Task<bool> UpdatePassengerInfoAsync(PassengerInfoUpdateResDto passenger)
        {
            throw new NotImplementedException();
        }
    }
}
