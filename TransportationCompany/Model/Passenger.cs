using System.ComponentModel.DataAnnotations.Schema;
using TransportationCompany.Model.Abstract;

namespace TransportationCompany.Model
{
    [Table("Passenger", Schema = "dbo")]
    public class Passenger : Common        
    {
        [Column("Name")]
        public string Name { get; set; }        

        [Column("Email")]
        public string Email { get; set; }

        [Column("Phone")]
        public string Phone { get; set; }

        public Passenger(string name, string email, string phone)
        {
            Name = name;            
            Email = email;
            Phone = phone;
        }
    }
}
