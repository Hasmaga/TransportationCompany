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

        [Column("DoB")]
        public DateTime? Dob { get; set; }

        [Column("CreatedDate")]
        public DateTime? CreatedDate { get; set; }

        [Column("Address")]
        public string? Address { get; set; }

        [Column("Avatar")]
        public string? Avatar { get; set; }

        public Passenger(string name, string email, string phone, DateTime? dob, DateTime? createdDate, string? address, string? avatar)
        {
            Name = name;
            Email = email;
            Phone = phone;
            Dob = dob;
            CreatedDate = createdDate;
            Address = address;
            Avatar = avatar;
        }
    }
}
