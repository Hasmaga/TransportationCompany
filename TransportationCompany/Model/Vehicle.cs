using System.ComponentModel.DataAnnotations.Schema;
using TransportationCompany.Model.Abstract;

namespace TransportationCompany.Model
{
    [Table("Vehicle", Schema = "dbo")]
    public class Vehicle : Common
    {
        [ForeignKey("CompanyId")]
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [ForeignKey("RouteTripId")]        
        public Guid RouteTripId { get; set; }
        public virtual RouteTrip RouteTrip { get; set; }

        [Column("RegistrationNumber")]
        public string RegistrationNumber { get; set; }

        [Column("Capacity")]
        public int Capacity { get; set; }
        public Vehicle(Guid companyId, Guid routeTripId, string registrationNumber, int capacity)
        {
            CompanyId = companyId;
            RouteTripId = routeTripId;            
            RegistrationNumber = registrationNumber;
            Capacity = capacity;
        }
    }
}
