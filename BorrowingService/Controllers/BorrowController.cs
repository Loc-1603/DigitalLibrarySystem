using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BorrowingService.Data;
using BorrowingService.Models;

[ApiController]
[Route("api/[controller]")]
public class BorrowController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;

    public BorrowController(AppDbContext context, IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
        _config = config;
    }

    [HttpPost]
    public async Task<IActionResult> BorrowBook(int userId, int bookId)
    {
        var client = _httpClientFactory.CreateClient();

        // Lấy URL từ config (sẽ thêm ở appsettings.json)
        var identityUrl = _config["ServiceUrls:Identity"];
        var bookUrl = _config["ServiceUrls:Book"];
        var notificationUrl = _config["ServiceUrls:Notification"];

        // 1. Check User
        var userResponse = await client.GetAsync($"{identityUrl}/api/users/{userId}");
        if (!userResponse.IsSuccessStatusCode) return BadRequest("User not found");
        var user = await userResponse.Content.ReadFromJsonAsync<UserDto>();

        // 2. Check Book stock
        var bookResponse = await client.GetAsync($"{bookUrl}/api/books/{bookId}");
        if (!bookResponse.IsSuccessStatusCode) return BadRequest("Book not found");
        var book = await bookResponse.Content.ReadFromJsonAsync<BookDto>();
        if (book.Stock <= 0) return BadRequest("Book out of stock");

        // 3. Silver rank limit
        if (user.Rank == "Silver")
        {
            var currentBorrowed = await _context.BorrowRecords
                .CountAsync(r => r.UserId == userId && r.ReturnDate == null);
            if (currentBorrowed >= 3) return BadRequest("Silver users can borrow max 3 books");
        }

        // 4. Save to BorrowingDB
        var record = new BorrowRecord
        {
            UserId = userId,
            BookId = bookId,
            BorrowDate = DateTime.Now
        };
        _context.BorrowRecords.Add(record);
        await _context.SaveChangesAsync();

        // 5. Update stock in Book Service
        var updateResponse = await client.PutAsync($"{bookUrl}/api/books/{bookId}/decrease-stock", null);
        if (!updateResponse.IsSuccessStatusCode)
        {
            return StatusCode(500, "Failed to update stock");
        }

        // 6. Notify
        await client.PostAsync($"{notificationUrl}/api/notifications?userId={userId}&message=Borrowed book {bookId}", null);

        return Ok("Borrow successful");
    }
}

// DTOs (đặt trong cùng file hoặc thư mục DTOs)
public class UserDto
{
    public int Id { get; set; }
    public string Rank { get; set; }
}
public class BookDto
{
    public int Id { get; set; }
    public int Stock { get; set; }
}