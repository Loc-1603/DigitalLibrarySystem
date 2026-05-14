using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookService.Data;
using BookService.Models;
using System;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly AppDbContext _context;
    public BooksController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBook(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) return NotFound();
        return Ok(book);
    }

    [HttpPut("{id}/decrease-stock")]
    public async Task<IActionResult> DecreaseStock(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) return NotFound("Book not found");

        if (book.Stock <= 0) return BadRequest("Out of stock");

        book.Stock -= 1;
        await _context.SaveChangesAsync();
        return Ok();
    }
}