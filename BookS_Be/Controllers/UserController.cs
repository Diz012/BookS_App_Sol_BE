using BookS_Be.Services.Interfaces;
using BookS_Be.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BookS_Be.Controllers;

/// <summary>
/// User management controller
/// </summary>
[ApiController]
[Route("api/user")]
[Produces("application/json")]
public class UserController(IUserService userService, JwtHelper jwtHelper) : ControllerBase
{
   
}
