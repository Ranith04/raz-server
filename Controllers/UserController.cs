using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RazServer.Entities;
using RazServer.Repositories;

namespace RazServer.Controllers;


[Produces("application/json")]
[ApiController]
[ApiExplorerSettings(GroupName = "User")]
[Route("v1/user")]
// [Authorize]
// [AuthClaim(NAuthClaims.ApplicationId, ApplicationEnumValue.Admin)]
// [AuthPermission(Perm.user_read)]
public class UserController : ControllerBase
{
    private readonly IUserRepository _user;
    public UserController(IUserRepository userRepository)
    {
        _user = userRepository;
    }

    /// <summary>
    /// Create User Account
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<UserAccount>> CreateUser([FromBody] UserAccount userAccount)
    {
        if (userAccount == null)
            return BadRequest("User account data is required.");

        var createdUser = await _user.Create(userAccount);

        if (createdUser == null)
            return StatusCode(500, "An error occurred while creating the user account.");

        return Ok(createdUser);
    }
}
