using System.ComponentModel.DataAnnotations.Schema;
using TransportationCompany.Model.Abstract;

namespace TransportationCompany.Model
{
    [Table("Company", Schema = "dbo")]
    public class Company : Common
    {
        [ForeignKey("PassengerId")]
        public Guid PassengerId { get; set; }
        public virtual Passenger Passenger { get; set; }

        [Column("Name")]
        public string Name { get; set; }
        
        [Column("Address")]
        public string Address { get; set; }
        
        [Column("Email")]
        public string Email { get; set; }
        
        [Column("Phone")]
        public string Phone { get; set; }

        public Company(Guid passengerId, string name, string address, string email, string phone)
        {
            PassengerId = passengerId;
            Name = name;
            Address = address;
            Email = email;
            Phone = phone;
        }
    }
}
