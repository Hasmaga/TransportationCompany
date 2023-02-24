using System.ComponentModel.DataAnnotations.Schema;
using TransportationCompany.Model.Abstract;

namespace TransportationCompany.Model
{
    [Table("Booking", Schema = "dbo")]
    public class Booking : Common
    {
        [ForeignKey("PassengerId")]
        public Guid PassengerId { get; set; }
        public virtual Passenger Passenger { get; set; }

        [ForeignKey("TripId")]
        public Guid TripId { get; set; }
        public virtual Trip Trip { get; set; }

        [Column("Seat")]
        public string Seat { get; set; }

        [Column("BookingDate")]
        public DateTime BookingDate { get; set; }

        [Column("Status")]
        public bool Status { get; set; }

        public Booking(Guid passengerId, Guid tripId, string seat, DateTime bookingDate, bool status)
        {
            PassengerId = passengerId;
            TripId = tripId;
            Seat = seat;
            BookingDate = bookingDate;
            Status = status;
        }        
    }
}
