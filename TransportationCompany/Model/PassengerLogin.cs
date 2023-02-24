using System.ComponentModel.DataAnnotations.Schema;
using TransportationCompany.Model.Abstract;

namespace TransportationCompany.Model
{
    [Table("PassengerLogin", Schema = "dbo")]
    public class PassengerLogin : Common
    {
        [ForeignKey("PassengerId")]
        public Guid PassengerId { get; set; }
        public virtual Passenger Passenger { get; set; }

        [Column("Status")]
        public bool Status { get; set; }

        [Column("PasswordHash")]
        public string PasswordHash { get; set; }

        [Column("PasswordSalt")]
        public string PasswordSalt { get; set; }

        public PassengerLogin(Guid passengerId, bool status, string passwordHash, string passwordSalt)
        {
            PassengerId = passengerId;
            Status = status;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }
    }
}
