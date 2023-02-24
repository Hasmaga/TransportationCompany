using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransportationCompany.Enum;
using TransportationCompany.Model.Dto;
using TransportationCompany.Repositories;

namespace TransportationCompany.Controllers
{
    [Route("ValidateApi/[controller]")]
    [ApiController]
    public class PassengerLoginController : ControllerBase
    {
        protected CommonResDto _resonse;

        private IPassengerLoginRepository _passengerLoginRepository;

        private readonly ILogger _logger;

        public PassengerLoginController(IPassengerLoginRepository passengerLoginRepository, ILogger<PassengerLoginController> logger)
        {
            _passengerLoginRepository = passengerLoginRepository;
            _logger = logger;
            _resonse = new CommonResDto();
        }

        [HttpPost("Login")]
        public async Task<ActionResult<CommonResDto>> LoginAsync(string Email, string Phone, string Password)
        {
            _logger.LogInformation("Login Passenger");
            if (Email != null)
            {
                try
                {
                    string token = await _passengerLoginRepository.LoginWithEmailAsync(Email, Password);
                    if (token != null)
                    {
                        _resonse.IsSuccess = true;
                        _resonse.Result = token;
                        _resonse.DisplayMessage = "Passenger Login Successfully";
                        return Ok(_resonse);
                    }
                    else
                    {
                        _resonse.IsSuccess = false;                        
                        _resonse.DisplayMessage = "Passenger Not Login";
                        return BadRequest(_resonse);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    return NotFound(ErrorCode.ACCOUNT_NOT_FOUND);
                }
            }
            else if (Phone != null)
            {
                try
                {
                    string token = await _passengerLoginRepository.LoginWithPhoneAsync(Phone, Password);
                    if (token!= null)
                    {
                        _resonse.IsSuccess = true;
                        _resonse.Result = token;
                        _resonse.DisplayMessage = "Passenger Login Successfully";
                        return Ok(_resonse);
                    }
                    else
                    {
                        _resonse.IsSuccess = false;
                        _resonse.DisplayMessage = "Passenger Not Login";
                        return BadRequest(_resonse);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    return NotFound(ErrorCode.ACCOUNT_NOT_FOUND);
                }
            }
            else return NotFound(ErrorCode.ACCOUNT_NOT_FOUND);
        }

        [HttpPost("Register Passenger Login")]
        public async Task<ActionResult<CommonResDto>> RegisterPassengerLoginAsync(RegistrationAccountResDto pasLogin)
        {
            _logger.LogInformation("Register Passenger Login");
            try
            {
                var result = await _passengerLoginRepository.RegistrationAccountAsync(pasLogin);
                if (result == true)
                {
                    _resonse.IsSuccess = true;                    
                    _resonse.DisplayMessage = "Passenger Login Added Successfully";
                    return Ok(_resonse);
                }
                else
                {
                    _resonse.IsSuccess = false;                    
                    _resonse.DisplayMessage = "Passenger Login Not Added";
                    return BadRequest(_resonse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(RegisterPassengerLoginAsync)}");
                _resonse.IsSuccess = false;
                _resonse.DisplayMessage = "Passenger Login Not Added Error";
                return BadRequest(_resonse);
            }
        }

    }
}
