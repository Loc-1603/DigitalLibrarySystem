using Microsoft.EntityFrameworkCore;
using BorrowingService.Models;

namespace BorrowingService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<BorrowRecord> BorrowRecords { get; set; }
    }
}