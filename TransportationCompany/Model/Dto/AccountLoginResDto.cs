namespace TransportationCompany.Model.Dto
{
    public class AccountLoginResDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public bool Status { get; set; }
        public string AuthType { get; set; }        
    }
}
