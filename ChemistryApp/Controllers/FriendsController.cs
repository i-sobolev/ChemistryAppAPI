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

    [Authorize]
    [HttpPost("Add")]
    public async Task<IActionResult> Post(int friendId)
    {
        try
        {
            var userId = User.GetLoggedInUserId<int>();

            var isRowExist = _entities.Friends.Any(u => u.UserId == userId && u.FriendId == friendId);

            if (isRowExist)
            {
                return BadRequest("User already has this friend");
            }

            _entities.Friends.Add(new Friend
            {
                UserId = userId,
                FriendId = friendId
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
}