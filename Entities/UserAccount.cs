#nullable disable

namespace RazServer.Entities;

public record UserAccount
{
    /// <summary>
    /// Refers to 'user' id
    /// </summary>
    public long Id { get; init; }
    public long? UserNumber { get; init; } = null;
    public string Username { get; init; } = null;
    public string Email { get; init; } = null;
    public long MobilePrimary { get; init; }
    public long? MobileSecondary { get; init; } = null;
    public string HashedPassword { get; set; }
    public string PlainPassword { get; init; } = null;
}
