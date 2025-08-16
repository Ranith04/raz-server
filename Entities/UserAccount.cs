#nullable disable

namespace RazServer.Entities;

public record UserAccount
{
    public long Id { get; init; }
    public string FirstName { get; init; }
    public string MiddleName { get; init; }
    public string LastName { get; init; }
    public string CountryCode { get; init; }
    public int MobileNumber { get; init; }
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


public record UserDocument
{
    public long Id { get; init; }
    public long UserId { get; init; }
    public long DocumentMediaId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}

public record UserBankAccount
{
    public long Id { get; init; }
    public long UserId { get; init; }
    public string BankName { get; init; }
    public string AccountNumber { get; init; }
    public string IfscCode { get; init; }
    public long BankMediaId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
