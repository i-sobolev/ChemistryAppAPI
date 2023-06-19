using ChemistryApp.Extensions;
using ChemistryApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChemistryApp.Controllers;

[Route("api/Chapters")]
[ApiController]
public class UserCompletedChapterController : ControllerBase
{
    private readonly ChemistryAppContext _entities;
    
    public UserCompletedChapterController() => _entities = new ChemistryAppContext();
    
    [Authorize]
    [HttpPost("Add")]
    public async Task<ActionResult> Post(int completedChapterId)
    {
        try
        {
            var userId = User.GetLoggedInUserId<int>();
            
            var isChapterExist = await _entities.CompletedChapters
                .AnyAsync(t => t.Id == completedChapterId);

            if (!isChapterExist)
            {
                _entities.CompletedChapters.Add(new CompletedChapter
                {
                    Id = completedChapterId
                });
            }
            
            var hasUserChapters = await _entities.UserCompletedChapters
                .AnyAsync(t => t.Id == completedChapterId && t.UserId == userId);

            if (!hasUserChapters)
            {
                _entities.UserCompletedChapters.Add(new UserCompletedChapter
                {
                    CompletedChapterId = completedChapterId,
                    UserId = userId
                });
            }

            if (!isChapterExist || !hasUserChapters)
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
    [HttpGet("GetCompletedChapters")]
    public async Task<ActionResult> GetCompletedChapters()
    {
        var userId = User.GetLoggedInUserId<int>();
        var completedChapters = await _entities.UserCompletedChapters
            .Where(u => u.UserId == userId)
            .Select(u => u.CompletedChapterId)
            .ToListAsync();

        return new ObjectResult(completedChapters);
    }
}