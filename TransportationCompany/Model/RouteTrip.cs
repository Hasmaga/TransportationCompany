using System.ComponentModel.DataAnnotations.Schema;
using TransportationCompany.Model.Abstract;

namespace TransportationCompany.Model
{
    [Table("RouteTrip", Schema = "dbo")]
    public class RouteTrip : Common
    {
        [ForeignKey("CompanyId")]
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [ForeignKey("FromLocationId")]
        public Guid FromLocationId { get; set; }
        public virtual Location From { get; set; }
        
        [ForeignKey("ToLocationId")]
        public Guid ToLocationId { get; set; }
        public virtual Location To { get; set; }

        [Column("Price")]
        public decimal Price { get; set; }

        public RouteTrip(Guid companyId, Guid fromLocationId, Guid toLocationId, decimal price)
        {
            CompanyId = companyId;
            FromLocationId = fromLocationId;
            ToLocationId = toLocationId;
            Price = price;
        }
    }
}
