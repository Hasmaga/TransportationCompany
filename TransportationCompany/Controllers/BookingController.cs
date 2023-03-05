
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransportationCompany.Model;
using TransportationCompany.Model.Dto;
using TransportationCompany.Repositories;
using TransportationCompany.Validation;

namespace TransportationCompany.Controllers
{
    [Route("bookingApi/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        protected CommonResDto _resonse;

        private IBookingRepository _bookingRepository;

        private readonly ILogger _logger;

        public BookingController(IBookingRepository bookingRepository, ILogger<BookingController> logger)
        {
            _bookingRepository = bookingRepository;
            _logger = logger;
            _resonse = new CommonResDto();
        }

        [HttpPost("BookingTripByCustomer")]
        public async Task<IActionResult> BookingTripByCustomer(BookingTripResDto book)
        {
            _logger.LogInformation("Booking Trip By Customer");
            if (!ValidationInputController.CheckSeatIsRightFormat(book.Seat))
            {
                _resonse.IsSuccess = false;
                _resonse.DisplayMessage = "Seat Invaild";
                return BadRequest(_resonse);
            }
            try
            {
                var result = await _bookingRepository.BookingTripByCustomerAsync(book);
                if (result != null)
                {
                    _resonse.IsSuccess = true;
                    _resonse.Result = result;
                    _resonse.DisplayMessage = "Booking Trip By Customer Successfully";
                    return Ok(_resonse);
                }
                else
                {
                    _resonse.IsSuccess = false;
                    _resonse.Result = result;
                    _resonse.DisplayMessage = "Booking Trip By Customer Not Added";
                    return BadRequest(_resonse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(BookingTripByCustomer)}");
                _resonse.IsSuccess = false;
                _resonse.DisplayMessage = "Booking Trip By Customer Not Added Error";
                return BadRequest(_resonse);
            }
        }

        [HttpPut, Authorize]
        [Route("CancelBookingByCustomer")]
        public async Task<IActionResult> CancelBookingByCustomer(Guid BookingId)
        {
            _logger.LogInformation("Cancel Booking By Customer");            
            try
            {
                var result = await _bookingRepository.CancelBookingByCustomerAsync(BookingId);
                if (result)
                {
                    _resonse.IsSuccess = true;
                    _resonse.Result = result;
                    _resonse.DisplayMessage = "Cancel Booking By Customer Successfully";
                    return Ok(_resonse);
                }
                else
                {
                    _resonse.IsSuccess = false;
                    _resonse.Result = result;
                    _resonse.DisplayMessage = "Cancel Booking By Customer Not Added";
                    return BadRequest(_resonse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(CancelBookingByCustomer)}");
                _resonse.IsSuccess = false;
                _resonse.DisplayMessage = "Cancel Booking By Customer Not Added Error";
                return BadRequest(_resonse);
            }
        }
        
        [HttpGet, Authorize]
        [Route("GetHistoryBookingByPassenger")]
        public async Task<ActionResult<List<HistoryBookingResDto>>> GetHistoryBookingByPassengerAsync()
        {
            _logger.LogInformation("Get History Booking By Passenger");
            try
            {
                var result = await _bookingRepository.GetHistoryBookingByPassengerAsync();
                if (result != null)
                {
                    _resonse.IsSuccess = true;
                    _resonse.Result = result;
                    _resonse.DisplayMessage = "Get History Booking By Passenger Successfully";
                    return Ok(_resonse);
                }
                else
                {
                    _resonse.IsSuccess = false;
                    _resonse.DisplayMessage = "There is no history booking In This Passenger";
                    return BadRequest(_resonse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(GetHistoryBookingByPassengerAsync)}");
                _resonse.IsSuccess = false;
                _resonse.DisplayMessage = "There is error while Get History Booking By Passenger";
                return BadRequest(_resonse);
            }
        }

        [HttpGet, Authorize]
        [Route("GetPresentBookingTicket")]
        public async Task<ActionResult<List<PresentBookingTicketResDto>>> GetPresentTicketByPassengerAsync()
        {
            _logger.LogInformation("GetPresentTicketByPassenger");
            try
            {
                var result = _bookingRepository.GetPresentBookingTicketByPassengerAsync();
                if (result != null)
                {
                    _resonse.IsSuccess = true;
                    _resonse.Result = result;
                    _resonse.DisplayMessage = "Get Present Booking By Passenger Successfully";
                    return Ok(_resonse);
                }
                else
                {
                    _resonse.IsSuccess = false;
                    _resonse.DisplayMessage = "There is no present booking In This Passenger";
                    return BadRequest(_resonse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(GetPresentTicketByPassengerAsync)}");
                _resonse.IsSuccess = false;
                _resonse.DisplayMessage = "There is error while Get Present Booking By Passenger";
                return BadRequest(_resonse);
            }
        }        
    }
}
