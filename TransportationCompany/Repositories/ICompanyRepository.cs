using Microsoft.Identity.Client;
using System.Reflection.Metadata.Ecma335;
using TransportationCompany.Model.Dto;

namespace TransportationCompany.Repositories
{
    public interface ICompanyRepository
    {
        Task<bool> CreateNewCompanyTripAsync(CreateCompanyTripResDto newTrip);
        Task<bool> CreateVehicleForCompanyAsync(CreateVehicleForCompanyResDto vehicle);
    }
}
