using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
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

        private async Task<PassengerLogin> GetAccountLogin()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid);
            }
            var acc = await _db.PassengerLogins.FindAsync(result);
            if (acc == null)
            {
                throw new Exception(ErrorCode.NOT_AUTHORIZED);
            }
            return acc;
        }

        public async Task<Passenger> GetPassengerId(string email, string phone)
        {
            try
            {
                var result = await _db.Passengers.FirstOrDefaultAsync(x => x.Email == email && x.Phone == phone);
                if (result != null)
                    return result;
                return null;
                
            } 
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ActionResult<BookingTripResDto>> BookingTripByCustomerAsync(BookingTripResDto book)
        {
            _logger.LogInformation("Booking Trip By Customer");
            try
            {
                var pas = await GetPassengerId(book.Email, book.Phone);   
                if (pas == null)
                {
                    throw new Exception(ErrorCode.ACCOUNT_NOT_FOUND);
                }
                Booking booking = new Booking(pas.Id, book.TripId, book.Seat, book.BookingDate, true);
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
                var pas = await GetAccountLogin();               
                var booking = await _db.Bookings.FirstOrDefaultAsync(x => x.Id == BookingId);
                if (booking == null)
                {
                    throw new Exception(ErrorCode.BOOKING_NOT_FOUND);
                }
                if (booking.PassengerId != pas.PassengerId)
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

        //public async Task<List<HistoryBookingResDto>> GetHistoryBookingByPassengerAsync()
        //{
        //    _logger.LogInformation("Get History Booking By Passenger");
        //    var pas = await GetAccountLogin();
        //    if (pas.PassengerId == null)
        //    {
        //        throw new Exception(ErrorCode.NOT_AUTHORIZED);
        //    }
        //    var result = (from r in _db.Passengers
        //                  join c in _db.Bookings on r.Id equals c.PassengerId
        //                  join t in _db.Trips on c.TripId equals t.Id
        //                  join v in _db.RouteTrips on t.RouteTripId equals v.Id                          
        //                  join y in _db.Vehicles on t.VehicleId equals y.Id
        //                  join z in _db.Companies on v.CompanyId equals z.Id                          
        //                  where r.Id == pas.PassengerId && c.Status == false
        //                  select new HistoryBookingResDto
        //                  {
        //                      Email = r.Email,
        //                      Name = r.Name,
        //                      Phone = r.Phone,
        //                      Seat = c.Seat,
        //                      BookingDate = c.BookingDate,
        //                      Status = c.Status,
        //                      DepartureTime = t.DepartureTime,
        //                      ArrivalTime = t.ArrivalTime,
        //                      Price = v.Price,
        //                      From= v.From.Name,
        //                      To = v.To.Name,
        //                      RegistrationNumber = y.RegistrationNumber,
        //                      Capacity = y.Capacity,
        //                      CompanyName = z.Name,
        //                      CompanyAddress = z.Address,
        //                      CompanyEmail = z.Email,
        //                      CompanyPhone = z.Phone
        //                  }).ToListAsync();
        //    if (result == null)
        //    {
        //        throw new Exception(ErrorCode.BOOKING_NOT_FOUND);
        //    }            
        //    return await result;
        //}       

        //public async Task<List<PresentBookingTicketResDto>> GetPresentBookingTicketByPassengerAsync()
        //{
        //    _logger.LogInformation("Get Present Ticket By Passenger");
        //    var pas = await GetAccountLogin();
        //    if (pas.PassengerId == null)
        //    {
        //        throw new Exception(ErrorCode.NOT_AUTHORIZED);
        //    }
        //    var result = (from r in _db.Passengers
        //                  join c in _db.Bookings on r.Id equals c.PassengerId
        //                  join t in _db.Trips on c.TripId equals t.Id
        //                  join v in _db.RouteTrips on t.RouteTripId equals v.Id
        //                  join y in _db.Vehicles on t.VehicleId equals y.Id
        //                  join z in _db.Companies on v.CompanyId equals z.Id
        //                  where r.Id == pas.PassengerId && c.Status == false
        //                  select new PresentBookingTicketResDto
        //                  {
        //                      Email = r.Email,
        //                      Name = r.Name,
        //                      Phone = r.Phone,
        //                      Seat = c.Seat,
        //                      BookingDate = c.BookingDate,
        //                      Status = c.Status,
        //                      DepartureTime = t.DepartureTime,
        //                      ArrivalTime = t.ArrivalTime,
        //                      Price = v.Price,
        //                      From = v.From.Name,
        //                      To = v.To.Name,                              
        //                      Capacity = y.Capacity,
        //                      CompanyName = z.Name,
        //                      CompanyAddress = z.Address,
        //                      CompanyEmail = z.Email,
        //                      CompanyPhone = z.Phone
        //                  }).ToListAsync();
        //    if (result == null)
        //    {
        //        throw new Exception(ErrorCode.BOOKING_NOT_FOUND);
        //    }
        //    return await result;
        //}
    }
}
