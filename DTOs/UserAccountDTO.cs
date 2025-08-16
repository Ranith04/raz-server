#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using RazServer.Entities;

namespace RazServer.DTOs;

public record CreateUserRequestDto
{
    [Required]
    [StringLength(50)]
    [JsonPropertyName("first_name")]
    public string FirstName { get; init; }

    [StringLength(50)]
    [JsonPropertyName("middle_name")]
    public string MiddleName { get; init; }

    [Required]
    [StringLength(50)]
    [JsonPropertyName("last_name")]
    public string LastName { get; init; }

    [StringLength(5)]
    [JsonPropertyName("country_code")]
    public string CountryCode { get; init; }

    [Required]
    [Range(1000000000, 9999999999, ErrorMessage = "Mobile number must be 10 digits.")]
    [JsonPropertyName("mobile_number")]
    public int MobileNumber { get; init; }

    [Required]
    [EmailAddress]
    [StringLength(255)]
    [JsonPropertyName("email")]
    public string Email { get; init; }

    [Required]
    [StringLength(255)]
    [JsonPropertyName("password")]
    public string Password { get; init; } // Plain password - will be hashed

    [Required]
    [DataType(DataType.Date)]
    [JsonPropertyName("date_of_birth")]
    public DateTime DateOfBirth { get; init; }

    [Required]
    [StringLength(100)]
    [JsonPropertyName("country_of_birth")]
    public string CountryOfBirth { get; init; }

    [StringLength(20)]
    [JsonPropertyName("gender")]
    public string Gender { get; init; }

    [JsonPropertyName("residential_address")]
    public string ResidentialAddress { get; init; }

    // Document Details
    [Required]
    [JsonPropertyName("documents")]
    public List<UserDocumentDto> Documents { get; init; } = new();

    // Bank Account Details
    [Required]
    [JsonPropertyName("bank_accounts")]
    public List<UserBankAccountDto> BankAccounts { get; init; } = new();
}

public record UserDocumentDto
{
    [Required]
    public long DocumentMediaId { get; init; }
}

public record UserBankAccountDto
{
    [Required]
    [StringLength(100)]
    public string BankName { get; init; }

    [Required]
    [StringLength(30)]
    public string AccountNumber { get; init; }

    [Required]
    [StringLength(20)]
    public string IfscCode { get; init; }

    [Required]
    public long BankMediaId { get; init; }
}

public record CreateUserResponseDto
{
    public UserAccount User { get; init; }
    public List<UserDocument> Documents { get; init; } = new();
    public List<UserBankAccount> BankAccounts { get; init; } = new();
}
