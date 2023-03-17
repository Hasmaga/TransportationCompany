using System.ComponentModel.DataAnnotations.Schema;
using TransportationCompany.Model.Abstract;

namespace TransportationCompany.Model
{
    [Table("RouteTrip", Schema = "dbo")]
    public class RouteTrip : Common
    {       

        [ForeignKey("FromLocationId")]
        public Guid FromLocationId { get; set; }
        public virtual Location From { get; set; }
        
        [ForeignKey("ToLocationId")]
        public Guid ToLocationId { get; set; }
        public virtual Location To { get; set; }      

        public RouteTrip(Guid fromLocationId, Guid toLocationId)
        {            
            FromLocationId = fromLocationId;
            ToLocationId = toLocationId;            
        }
    }
}
