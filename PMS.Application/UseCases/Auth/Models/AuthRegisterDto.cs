namespace PMS.Application.UseCases.Auth.Models
{
    public class AuthRegisterDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
     
    }
}
