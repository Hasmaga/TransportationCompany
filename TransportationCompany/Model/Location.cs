using System.ComponentModel.DataAnnotations.Schema;
using TransportationCompany.Model.Abstract;

namespace TransportationCompany.Model
{
    [Table("Location", Schema = "dbo")]
    public class Location : Common
    {
        [Column("Name")]
        public string Name { get; set; }

        public ICollection<RouteTrip> FromLocation { get; set; } = new List<RouteTrip>();
        public ICollection<RouteTrip> ToLocation { get; set; } = new List<RouteTrip>();

        public Location(string name)
        {
            Name = name;
        }
    }
}
