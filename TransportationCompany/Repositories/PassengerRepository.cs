using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportationCompany.DbContexts;
using TransportationCompany.Enum;
using TransportationCompany.Model;
using TransportationCompany.Model.Dto;

namespace TransportationCompany.Repositories
{
    public class PassengerRepository : IPassengerRepository
    {
        //Add ApplicationDbContext
        private readonly ApplicationDbContext _db;

        //Add AutoMapper to map Dto to Model
        private IMapper _mapper;

        // Add Logger to log every event to the console for tracking purpose
        private readonly ILogger _logger;

        private readonly IConfiguration _configuration;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public PassengerRepository(ApplicationDbContext db, IMapper mapper, ILogger<PassengerRepository> logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        //Add Passenger
        public async Task<ActionResult<AddPassengerResDto>> AddPassengerAsync(AddPassengerResDto passenger)
        {
            try
            {
                if (IsPassengerExistAsync(passenger.Email, passenger.Phone).Result)
                {
                    throw new Exception(ErrorCode.PASSENGER_HAD_EXIST);
                }                
                Passenger pass = new Passenger(passenger.Name, passenger.Email, passenger.Phone);
                await _db.Passengers.AddAsync(pass);
                await _db.SaveChangesAsync();
                return passenger;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding passenger");
                throw new Exception(ErrorCode.PASSENGER_ADD_ERROR);
            }
        }
        
        //Check if passenger exist though email and phone
        public async Task<bool> IsPassengerExistAsync(string email, string phone)
        {
            try
            {
                var passenger = await _db.Passengers.FirstOrDefaultAsync(p => p.Email == email || p.Phone == phone);
                if (passenger != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while checking passenger");
                throw new Exception(ErrorCode.CHECK_PASSENGER_ERROR);
            }
        }        
    }
}
