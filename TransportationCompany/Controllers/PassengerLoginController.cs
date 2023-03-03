using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using System.Text.RegularExpressions;
using TransportationCompany.Enum;
using TransportationCompany.Model.Dto;
using TransportationCompany.Repositories;
using TransportationCompany.Validation;

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

        // Login Api (url = http://localhost:5000/ValidateApi/PassengerLogin/Login)

        [HttpPost("Login")]
        public async Task<ActionResult<CommonResDto>> LoginAsync(string? Email, string? Phone, string Password)
        {
            // Log Information in Console
            _logger.LogInformation("Login Passenger");
            // Check Email and Phone is not null and check format input and resonse by file Json by url
            if (Email != null)
            {
                if (!ValidationInputController.CheckEmailIsRightFormat(Email))
                {
                    _resonse.IsSuccess = false;
                    _resonse.DisplayMessage = "Email Invaild";
                    return BadRequest(_resonse);
                }
                try
                {
                    string token = await _passengerLoginRepository.LoginWithEmailAsync(Email, Password);
                    if (token != null)
                    {
                        _resonse.IsSuccess = true;
                        _resonse.Result = token;
                        _resonse.DisplayMessage = "Account Login Successfully";
                        return Ok(_resonse);
                    }
                    else
                    {
                        _resonse.IsSuccess = false;                        
                        _resonse.DisplayMessage = "Account Not Found";
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
                if(!ValidationInputController.CheckPhoneVaild(Phone))
                {
                    _resonse.IsSuccess = false;
                    _resonse.DisplayMessage = "Phone Invaild";
                    return BadRequest(_resonse);
                }                
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


        // Register account

        [HttpPost("RegisterPassengerLogin")]
        public async Task<ActionResult<CommonResDto>> RegisterPassengerLoginAsync(RegistrationAccountResDto pasLogin)
        {
            _logger.LogInformation("Register Passenger Login");           
            if (!ValidationInputController.CheckEmailIsRightFormat(pasLogin.Email) || !ValidationInputController.CheckPhoneVaild(pasLogin.Phone) || !ValidationInputController.IsValidPassword(pasLogin.Password))
            {
                if (!ValidationInputController.CheckEmailIsRightFormat(pasLogin.Email))           
                    _resonse.DisplayMessage = "Email Invaild ";         
                if (!ValidationInputController.CheckPhoneVaild(pasLogin.Phone))
                    _resonse.DisplayMessage += "Phone Invaild ";
                if (!ValidationInputController.IsValidPassword(pasLogin.Password))
                    _resonse.DisplayMessage += "Password Invaild ";
                _resonse.IsSuccess = false;
                return BadRequest(_resonse);             
            }            
            try
            {
                var result = await _passengerLoginRepository.RegistrationAccountAsync(pasLogin);
                if (result == true)
                {
                    _resonse.IsSuccess = true;                    
                    _resonse.DisplayMessage = "Account Added Successfully";
                    return Ok(_resonse);
                }
                else
                {
                    _resonse.IsSuccess = false;                    
                    _resonse.DisplayMessage = "Account is already register";
                    return BadRequest(_resonse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(RegisterPassengerLoginAsync)}");
                _resonse.IsSuccess = false;
                _resonse.DisplayMessage = "Function register account is error!";
                return BadRequest(_resonse);
            }
        }
    }
}
