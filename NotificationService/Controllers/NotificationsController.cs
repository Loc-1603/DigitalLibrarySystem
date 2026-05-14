using Microsoft.AspNetCore.Mvc;
using NotificationService.Data;
using NotificationService.Models;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly AppDbContext _context;
    public NotificationsController(AppDbContext context) => _context = context;

    [HttpPost]
    public async Task<IActionResult> CreateNotification(int userId, string message)
    {
        var notif = new Notification
        {
            UserId = userId,
            Message = message,
            CreatedAt = DateTime.Now
        };
        _context.Notifications.Add(notif);
        await _context.SaveChangesAsync();
        return Ok();
    }
}