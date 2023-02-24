using System.ComponentModel.DataAnnotations.Schema;
using TransportationCompany.Model.Abstract;

namespace TransportationCompany.Model
{
    [Table("Company", Schema = "dbo")]
    public class Company : Common
    {
        [Column("Name")]
        public string Name { get; set; }
        
        [Column("Address")]
        public string Address { get; set; }
        
        [Column("Email")]
        public string Email { get; set; }
        
        [Column("Phone")]
        public string Phone { get; set; }
        
        public Company(string name, string address, string email, string phone)
        {
            Name = name;
            Address = address;
            Email = email;
            Phone = phone;
        }
    }
}
