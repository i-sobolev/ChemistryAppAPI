using ChemistryApp.Extensions;
using ChemistryApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChemistryApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FriendsController : ControllerBase
{
    private readonly ChemistryAppContext _entities;
    
    public FriendsController() => _entities = new ChemistryAppContext();
    
    [Authorize]
    [HttpGet("Get")]
    public async Task<ActionResult> GetFriends()
    {
        var userId = User.GetLoggedInUserId<int>();
        var friends = await _entities.Friends
            .Where(u => u.UserId == userId)
            .Select(u => u.FriendNavigation.Name)
            .ToListAsync();

        return new ObjectResult(friends);
    }
}