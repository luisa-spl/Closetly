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
        public void CreateRating(RatingCreateDTO rating)
        {
            TbRating newRating = new TbRating();
            newRating.OrderId = rating.OrderId;
            newRating.Rate = rating.Rate;
            _context.TbRatings.Add(newRating);
            _context.SaveChanges();
        }
    }
}
