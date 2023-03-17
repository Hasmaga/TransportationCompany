namespace TransportationCompany.Model.Dto
{
    public class CreateVehicleForCompanyResDto
    {
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string RegistrationNumber { get; set; }
        public int Capacity { get; set; }
    }
}
