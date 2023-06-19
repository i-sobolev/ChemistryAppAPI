using ChemistryApp.Extensions;
using ChemistryApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChemistryApp.Controllers;

[Route("api/Tasks")]
[ApiController]
public class UserCompletedTaskController : ControllerBase
{
    private readonly ChemistryAppContext _entities;
    
    public UserCompletedTaskController() => _entities = new ChemistryAppContext();

    [Authorize]
    [HttpPost("Add")]
    public async Task<ActionResult> Post(int completedTaskId)
    {
        try
        {
            var userId = User.GetLoggedInUserId<int>();

            var isTaskExist = await _entities.CompletedTasks
                .AnyAsync(t => t.Id == completedTaskId);

            if (!isTaskExist)
            {
                _entities.CompletedTasks.Add(new CompletedTask
                {
                    Id = completedTaskId
                });
            }

            var hasUserTasks = await _entities.UserCompletedTasks
                .AnyAsync(t => t.Id == completedTaskId && t.UserId == userId);

            if (!hasUserTasks)
            {
                _entities.UserCompletedTasks.Add(new UserCompletedTask
                {
                    UserId = userId,
                    CompletedTaskId = completedTaskId
                });
            }

            if (!hasUserTasks || !isTaskExist)
            {
                await _entities.SaveChangesAsync();
            }
            
            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return BadRequest();
        }
    }
    
    [Authorize]
    [HttpGet("GetCompletedTasks")]
    public async Task<ActionResult> GetCompletedTasks()
    {
        var userId = User.GetLoggedInUserId<int>();
        var completedTasks = await _entities.UserCompletedTasks
            .Where(u => u.UserId == userId)
            .Select(u => u.CompletedTaskId)
            .ToListAsync();

        return new ObjectResult(completedTasks);
    }
    
    [Authorize]
    [HttpGet("GetLeaders")]
    public async Task<ActionResult> GetLeaders()
    {
        var userId = User.GetLoggedInUserId<int>();
        var friendIds = await _entities.Friends.Where(u => u.UserId == userId)
            .Select(f => f.FriendId)
            .ToListAsync();

        var leaders = new List<Leader>();

        foreach (var friendId in friendIds)
        {
            var amount = await _entities.UserCompletedTasks
                .CountAsync(u => u.UserId == friendId);
            var friend = await _entities.Users
                .FirstOrDefaultAsync(u => u.Id == friendId);

            leaders.Add(new Leader
            {
                UserName = friend?.Name,
                CompletedTasksAmount = amount
            });
        }

        var userCompletesTasksAmount = await _entities.UserCompletedTasks
            .CountAsync(u => u.UserId == userId);
        var user = await _entities.Users
            .FirstOrDefaultAsync(u => u.Id == userId);
        
        leaders.Add(new Leader
        {
            UserName = user?.Name,
            CompletedTasksAmount = userCompletesTasksAmount
        });

        return new ObjectResult(leaders.OrderByDescending(u => u.CompletedTasksAmount));
    }
}