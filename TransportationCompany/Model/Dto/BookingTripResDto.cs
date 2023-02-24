namespace TransportationCompany.Model.Dto
{
    public class BookingTripResDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }        
        public Guid TripId { get; set; }
        public string Seat { get; set; }
        public DateTime BookingDate { get; set; }        
    }
}
