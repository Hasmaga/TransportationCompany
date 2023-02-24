using System.ComponentModel.DataAnnotations.Schema;
using TransportationCompany.Model.Abstract;

namespace TransportationCompany.Model
{
    [Table("Trip", Schema = "dbo")]
    public class Trip : Common
    {
        [Column("VehicleId")]
        public Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        [Column("RouteTripId")]
        public Guid RouteTripId { get; set; }
        public RouteTrip RouteTrip { get; set; }
        
        [Column("DepartureTime")]
        public DateTime DepartureTime { get; set; }

        [Column("ArrivalTime")]
        public DateTime ArrivalTime { get; set; }

        [Column("Price")]
        public decimal Price { get; set; }

        public Trip(Guid vehicleId, Guid routeTripId, DateTime departureTime, DateTime arrivalTime, decimal price)
        {
            VehicleId = vehicleId;
            RouteTripId = routeTripId;
            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
            Price = price;
        }
    }
}
