using Closetly.DTO;
using Closetly.Models;
using Closetly.Repository.Interface;
using System.Diagnostics.CodeAnalysis;

namespace Closetly.Repository
{
    [ExcludeFromCodeCoverage]
    public class RatingRepository : IRatingRepository
    {
        private readonly PostgresContext _context;

        public RatingRepository(PostgresContext context)
        {
            _context = context;
        }
        public async Task CreateRating(RatingCreateDTO rating)
        {
            TbRating newRating = new TbRating();
            newRating.OrderId = rating.OrderId;
            newRating.Rate = rating.Rate;
            _context.TbRatings.Add(newRating);
            await _context.SaveChangesAsync();
        }
    }
}
