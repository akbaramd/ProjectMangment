namespace PMS.Application.UseCases.User.Models;

public class UserProfileDto
{
    public Guid Id { get; set; }        // UserEntity's unique identifier
    public string FullName { get; set; }    // UserEntity's full name
    public string PhoneNumber { get; set; } // UserEntity's phone number
    public string Email { get; set; }       // UserEntity's email address

}