using Closetly.DTO;
using Closetly.Models;
using Closetly.Repository.Interface;

namespace Closetly.Repository
{
    public class RatingRepository : IRatingRepository
    {
        private readonly PostgresContext _context;

        public RatingRepository(PostgresContext context)
        {
            _context = context;
        }
        public void CreateRating(RatingDTO rating)
        {
            TbRating newRating = new TbRating();
            newRating.Rate = rating.Rate;
            newRating.CreatedAt = rating.CreatedAt;
            _context.SaveChanges();
        }
    }
}
