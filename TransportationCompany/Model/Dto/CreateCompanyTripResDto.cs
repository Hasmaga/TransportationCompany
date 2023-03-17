namespace TransportationCompany.Model.Dto
{
    public class CreateCompanyTripResDto
    {
        public Guid VehicalId { get; set; }
        public Guid RouteTripId { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
        public Guid CompanyId { get; set; }
        public decimal Price { get; set; }
    }
}
