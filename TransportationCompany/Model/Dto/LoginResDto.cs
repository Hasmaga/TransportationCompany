namespace TransportationCompany.Model.Dto
{
    public class LoginResDto
    {
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public bool Status { get; set; }
    }
}
