#nullable disable

namespace RazServer.Entities;

public record UserAccount
{
    public long Id { get; init; }
    public string FirstName { get; init; }
    public string MiddleName { get; init; }
    public string LastName { get; init; }
    public string MobileNumber { get; init; }
    public string Email { get; init; }
    public string PasswordHash { get; init; }
    public DateTime DateOfBirth { get; init; }
    public string CountryOfBirth { get; init; }
    public string Gender { get; init; }
    public string ResidentialAddress { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public bool IsActive { get; init; }
}
