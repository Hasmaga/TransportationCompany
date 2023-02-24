using Microsoft.AspNetCore.Mvc;
using TransportationCompany.Model;
using TransportationCompany.Model.Dto;

namespace TransportationCompany.Repositories
{
    public interface IPassengerRepository
    {
        Task<ActionResult<AddPassengerResDto>> AddPassengerAsync(AddPassengerResDto passenger);
    }
}
