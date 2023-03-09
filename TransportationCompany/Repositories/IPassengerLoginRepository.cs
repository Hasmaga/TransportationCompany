using TransportationCompany.Model.Dto;

namespace TransportationCompany.Repositories
{
    public interface IPassengerLoginRepository
    {
        Task<string> LoginWithEmailAsync(string Email, string Password);
        Task<string> LoginWithPhoneAsync(string Phone, string Password);
        Task<bool> RegistrationAccountAsync(RegistrationAccountResDto passenger);
        Task<bool> UpdatePassengerInfoAsync(PassengerInfoUpdateResDto passenger);
    }
}
