using TransportationCompany.Model.Dto;

namespace TransportationCompany.Repositories
{
    public interface IAdminAccRepository
    {
        Task<bool> CreateNewAccountForTransportation(RegisterCompanyResDto newCom);
    }
}
