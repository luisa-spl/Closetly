using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Closetly.Data;

[ExcludeFromCodeCoverage]
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}
