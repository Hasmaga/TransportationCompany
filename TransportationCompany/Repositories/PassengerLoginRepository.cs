﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
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
        public PassengerLoginRepository(ApplicationDbContext db, IMapper mapper, ILogger<PassengerRepository> logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
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

        private string GenerateToken(Guid Id, String Name, String Email, String Phone)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, Id.ToString()),
                new Claim(ClaimTypes.Name, Name),
                new Claim(ClaimTypes.Email, Email),
                new Claim(ClaimTypes.MobilePhone, Phone)
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

        public async Task<string> LoginWithEmailAsync(string email, string password)
        {
            _logger.LogInformation("Login Passenger with Email");
            
            var query = (from r in _db.Passengers
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
                             c.PasswordSalt
                         }).FirstOrDefault();
            if (query == null) throw new UnauthorizedAccessException(ErrorCode.ACCOUNT_NOT_FOUND);
            if (!VerifyPassHash(password, query.PasswordHash, query.PasswordSalt)) throw new UnauthorizedAccessException(ErrorCode.PASSWORD_INCORRECT);
            if (query.Status == false) throw new UnauthorizedAccessException(ErrorCode.PASSENGER_NOT_ACTIVE);
            string token = GenerateToken(query.Id, query.Name, query.Email, query.Phone);
            return token;
        }

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
                             c.PasswordSalt
                         }).FirstOrDefault();
            if (query == null) throw new UnauthorizedAccessException(ErrorCode.ACCOUNT_NOT_FOUND);
            if (!VerifyPassHash(Password, query.PasswordHash, query.PasswordSalt)) throw new UnauthorizedAccessException(ErrorCode.PASSWORD_INCORRECT);
            if (query.Status == false) throw new UnauthorizedAccessException(ErrorCode.PASSENGER_NOT_ACTIVE);
            string token = GenerateToken(query.Id, query.Name, query.Email, query.Phone);
            return token;            
        }
        
        public async Task<bool> RegistrationAccountAsync(RegistrationAccountResDto passenger)
        {
            _logger.LogInformation("Registration Passenger");
            try
            {
                var result = await _db.Passengers.FirstOrDefaultAsync(x => x.Email == passenger.Email && x.Name == passenger.Name && x.Phone == passenger.Phone);
                if (result == null)
                {
                    _logger.LogInformation("Create Passenger");
                    Passenger pas = new Passenger(passenger.Name, passenger.Email, passenger.Phone);
                    await _db.Passengers.AddAsync(pas);
                    await _db.SaveChangesAsync();
                    _logger.LogInformation("Create Passenger Login");
                    CreatePassHash(passenger.Password, out byte[] passwordHash, out byte[] passwordSalt);
                    PassengerLogin pasLogin = new PassengerLogin(pas.Id, true, Convert.ToBase64String(passwordHash), Convert.ToBase64String(passwordSalt));
                    await _db.PassengerLogins.AddAsync(pasLogin);
                    await _db.SaveChangesAsync();
                    return true;
                }
                else if (result != null)
                {
                    _logger.LogInformation("Create Passenger Login");
                    CreatePassHash(passenger.Password, out byte[] passwordHash, out byte[] passwordSalt);
                    PassengerLogin pasLogin = new PassengerLogin(result.Id, true, Convert.ToBase64String(passwordHash), Convert.ToBase64String(passwordSalt));
                    await _db.PassengerLogins.AddAsync(pasLogin);
                    await _db.SaveChangesAsync();
                    return true;
                }                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error While Registration Passenger");
                return false;
            }
            return false;
        }        
    }
}