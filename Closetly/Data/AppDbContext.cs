using Microsoft.EntityFrameworkCore;

namespace Closetly.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}
