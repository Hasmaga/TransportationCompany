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
    }
}
