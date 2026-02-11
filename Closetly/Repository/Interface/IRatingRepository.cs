using Closetly.DTO;

namespace Closetly.Repository.Interface
{
    public interface IRatingRepository
    {
        public Task CreateRating(RatingCreateDTO rating);
    }
}
