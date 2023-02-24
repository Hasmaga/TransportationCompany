using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TransportationCompany.DbContexts;
using TransportationCompany.Enum;
using TransportationCompany.Model;
using TransportationCompany.Model.Dto;

namespace TransportationCompany.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _db;
        private IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BookingRepository(ApplicationDbContext db, IMapper mapper, ILogger<BookingRepository> logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        private Task<Guid> GetPassengerLoginIdAsync()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            return Task.FromResult(Guid.Parse(result));            
        }

        private async Task<Guid> GetUserId(String Name, String Email, String Phone)
        {
            var user = _db.Passengers.FirstOrDefault(x => x.Name == Name && x.Email == Email && x.Phone == Phone);
            if (user == null)
            {
                Passenger passenger = new Passenger(Name, Email, Phone);
                await _db.Passengers.AddAsync(passenger);
                await _db.SaveChangesAsync();
                return passenger.Id;
            }
            return user.Id;
        }
        

        public async Task<ActionResult<BookingTripResDto>> BookingTripByCustomerAsync(BookingTripResDto book)
        {
            _logger.LogInformation("Booking Trip By Customer");
            try
            {
                Guid PassengerId = await GetUserId(book.Name, book.Email, book.Phone);
                Booking booking = new Booking(PassengerId, book.TripId, book.Seat, book.BookingDate, true);
                await _db.Bookings.AddAsync(booking);
                await _db.SaveChangesAsync();
                return book;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while booking trip");
                throw new Exception(ErrorCode.BOOKING_TRIP_ERROR);
            }                    
        }

        public async Task<bool> CancelBookingByCustomerAsync(Guid BookingId)
        {
            _logger.LogInformation("Cancel Booking By Customer");
            try
            {
                Guid PassengerId = await GetPassengerLoginIdAsync();
                
                var booking = await _db.Bookings.FirstOrDefaultAsync(x => x.Id == BookingId);
                if (booking == null)
                {
                    throw new Exception(ErrorCode.BOOKING_NOT_FOUND);
                }
                if (booking.PassengerId != PassengerId)
                {
                    throw new Exception(ErrorCode.NOT_AUTHORIZED);
                }
                booking.Status = false;
                _db.Bookings.Update(booking);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while cancelling booking");
                throw new Exception(ErrorCode.BOOKING_CANCEL_ERROR);
            }
        }        
    }
}
