#nullable disable

using System.Data;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RazServer.DTOs;
using RazServer.Entities;
using RazServer.Repositories;

namespace RazServer.Controllers;

[Produces("application/json")]
[ApiController]
[ApiExplorerSettings(GroupName = "User")]
[Route("v1/user")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _user;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserRepository userRepository, ILogger<UserController> logger)
    {
        _user = userRepository;
        _logger = logger;
    }

    /// <summary>
    /// Create User Account with Documents and Bank Details
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<CreateUserResponseDto>> CreateUser([FromBody] CreateUserRequestDto request)
    {
        if (request == null)
            return BadRequest("User data is required.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        async Task<CreateUserResponseDto> Run(IDbConnection con, IDbTransaction tx)
        {
            var now = DateTimeOffset.UtcNow;

            // 1. Create User Account
            var hashedPassword = HashPassword(request.Password);
            var userAccount = new UserAccount
            {
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                CountryCode = request.CountryCode,
                MobileNumber = request.MobileNumber,
                Email = request.Email,
                PasswordHash = hashedPassword,
                DateOfBirth = request.DateOfBirth,
                CountryOfBirth = request.CountryOfBirth,
                Gender = request.Gender,
                ResidentialAddress = request.ResidentialAddress,
                CreatedAt = now,
                UpdatedAt = now,
                IsActive = true
            };

            var createdUser = await _user.Create(userAccount, con);
            if (createdUser == null)
                throw new Exception("Failed to create user account");

            // 2. Create User Documents
            var createdDocuments = new List<UserDocument>();
            foreach (var docDto in request.Documents)
            {
                var document = new UserDocument
                {
                    UserId = createdUser.Id,
                    DocumentMediaId = docDto.DocumentMediaId,
                    CreatedAt = now,
                    UpdatedAt = now
                };

                var createdDocument = await _user.CreateDocument(document, con);
                if (createdDocument != null)
                    createdDocuments.Add(createdDocument);
            }

            // 3. Create Bank Accounts
            var createdBankAccounts = new List<UserBankAccount>();
            foreach (var bankDto in request.BankAccounts)
            {
                var bankAccount = new UserBankAccount
                {
                    UserId = createdUser.Id,
                    BankName = bankDto.BankName,
                    AccountNumber = bankDto.AccountNumber,
                    IfscCode = bankDto.IfscCode,
                    BankMediaId = bankDto.BankMediaId,
                    CreatedAt = now,
                    UpdatedAt = now
                };

                var createdBankAccount = await _user.CreateBankAccount(bankAccount, con);
                if (createdBankAccount != null)
                    createdBankAccounts.Add(createdBankAccount);
            }

            return new CreateUserResponseDto
            {
                User = createdUser,
                Documents = createdDocuments,
                BankAccounts = createdBankAccounts
            };
        }

        try
        {
            var result = await (_user as BaseRepository).RunInTransaction(Run);

            _logger.LogInformation($"Created user with email: {request.Email}, documents count: {request.Documents.Count}, bank accounts count: {request.BankAccounts.Count}");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating user with email: {request.Email}");
            return StatusCode(500, new { message = "An error occurred while creating the user account.", error = ex.Message });
        }
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "your_salt_here"));
        return Convert.ToBase64String(hashedBytes);
    }
}
