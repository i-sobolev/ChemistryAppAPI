using System.Security.Claims;
using ChemistryApp.Extensions;
using ChemistryApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChemistryApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IJwtTokenManager _tokenManager;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ChemistryAppContext _entities;

    public UserController(IJwtTokenManager tokenManager, IHttpContextAccessor contextAccessor)
    {
        _tokenManager = tokenManager;
        _contextAccessor = contextAccessor;
        _entities = new ChemistryAppContext();
    }

    [AllowAnonymous]
    [HttpGet("Authenticate")]
    public async Task<IActionResult> Authenticate(string login, string password)
    {
        var token = await _tokenManager.Authenticate(login, password);

        if (token == null)
        {
            return Unauthorized();
        }

        return Ok(token);
    }

    [Authorize]
    [HttpGet("GetInfo")]
    public async Task<ActionResult> GetUserInfo()
    {
        var userId = User.GetLoggedInUserId<int>();

        var completedChaptersAmount = await _entities.UserCompletedChapters
            .CountAsync(u => u.UserId == userId);
        
        var completedTasksAmount = await _entities.UserCompletedTasks
            .CountAsync(u => u.UserId == userId);

        var user = await _entities.Users
            .Where(u => u.Id == userId)
            .Select(u => new UserInfo
            {
                Name = u.Name,
                Id = u.Id,
                CompletedChaptersAmount = completedChaptersAmount,
                CompletedTasksAmount = completedTasksAmount
            })
            .FirstOrDefaultAsync();
        
        return new ObjectResult(user);
    }

    [HttpPost("Create")]
    public async Task<ActionResult> AddUser([FromBody] UserInfo userInfo)
    {
        try
        {
            if (_entities.Users.Any(u => u.Login == userInfo.Login))
            {
                return BadRequest("User with this login already exists");
            }
            
            _entities.Users.Add(new User
            {
                Login = userInfo.Login,
                Password = userInfo.Password,
                Name = userInfo.Name
            });

            await _entities.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return BadRequest();
        }
    }

    [Authorize]
    [HttpGet("GetOtherUsers")]
    public async Task<IActionResult> GetUsers()
    {
        var userId = User.GetLoggedInUserId<int>();

        var friends = await _entities.Users.Where(u => u.Id != userId)
            .ToListAsync();

        return new ObjectResult(friends);
    }
}