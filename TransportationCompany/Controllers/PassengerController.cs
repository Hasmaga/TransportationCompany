using Microsoft.AspNetCore.Mvc;
using TransportationCompany.Model;
using TransportationCompany.Model.Dto;
using TransportationCompany.Repositories;

namespace TransportationCompany.Controllers
{
    [Route("passengerApi/[controller]")]
    [ApiController]
    public class PassengerController : ControllerBase
    {
        protected CommonResDto _resonse;

        private IPassengerRepository _passengerRepository;

        private readonly ILogger _logger;

        public PassengerController(IPassengerRepository passengerRepository, ILogger<PassengerController> logger)
        {
            _passengerRepository = passengerRepository;
            _logger = logger;
            _resonse = new CommonResDto();
        }

        [HttpPost("AddPassenger")]
        public async Task<IActionResult> AddPassenger(AddPassengerResDto passenger)
        {
            _logger.LogInformation("Add Passenger");
            try
            {
                var result = await _passengerRepository.AddPassengerAsync(passenger);
                if (result != null)
                {
                    _resonse.IsSuccess = true;
                    _resonse.Result = result;
                    _resonse.DisplayMessage = "Passenger Added Successfully";
                    return Ok(_resonse);
                }
                else
                {
                    _resonse.IsSuccess = false;
                    _resonse.Result = result;
                    _resonse.DisplayMessage = "Passenger Not Added";
                    return BadRequest(_resonse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(AddPassenger)}");
                _resonse.IsSuccess = false;
                _resonse.DisplayMessage = "Passenger Not Added Error";
                return BadRequest(_resonse);
            }
        }
    }
}
