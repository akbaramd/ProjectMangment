namespace PMS.Application.DTOs;

public class UserProfileDto
{
    public Guid Id { get; set; }        // User's unique identifier
    public string FullName { get; set; }    // User's full name
    public string PhoneNumber { get; set; } // User's phone number
    public string Email { get; set; }       // User's email address

    public List<string> Roles { get; set; }
    public List<string> Permissions { get; set; }
    // Add other profile fields as needed
}