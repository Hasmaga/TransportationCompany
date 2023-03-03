namespace TransportationCompany.Model.Dto
{
    public class PresentBookingTicketResDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Seat { get; set; }
        public DateTime BookingDate { get; set; }
        public bool Status { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal Price { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int Capacity { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyEmail { get; set; }
    }
}
