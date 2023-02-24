using System.ComponentModel.DataAnnotations.Schema;
using TransportationCompany.Model.Abstract;

namespace TransportationCompany.Model
{
    [Table("PassengerComment", Schema = "dbo")]
    public class PassengerComment : Common 
    {
        [ForeignKey("PassengerId")]
        public Guid PassengerId { get; set; }
        public virtual Passenger Passenger { get; set; }

        [Column("Comment")]
        public string Comment { get; set; }

        public PassengerComment(Guid passengerId, string comment)
        {
            PassengerId = passengerId;
            Comment = comment;
        }
    }
}
