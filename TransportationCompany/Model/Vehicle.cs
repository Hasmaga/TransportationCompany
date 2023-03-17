using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using TransportationCompany.Model.Abstract;

namespace TransportationCompany.Model
{
    [Table("Vehicle", Schema = "dbo")]
    public class Vehicle : Common
    {
        [ForeignKey("CompanyId")]
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("RegistrationNumber")]
        public string RegistrationNumber { get; set; }

        [Column("Capacity")]
        public int Capacity { get; set; }
        public Vehicle(Guid companyId, string name, string registrationNumber, int capacity)
        {
            CompanyId = companyId;
            Name = name;
            RegistrationNumber = registrationNumber;
            Capacity = capacity;
        }
    }
}
