using TransportationCompany.Model.Dto;

namespace TransportationCompany.Repositories
{
    public interface IAdminAccRepository
    {
        Task<bool> CreateNewAccountForTransportation(RegisterCompanyResDto newCom);
        Task<List<AccountLoginResDto>> GetAllAccount();
        Task<bool> ChangeStatusAccount(Guid Id);       
    }
}
