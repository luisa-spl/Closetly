using Closetly.DTO;

namespace Closetly.Services.Interface
{
    public interface IRatingService
    {
        public Task CreateRating(RatingCreateDTO rating);

    }
}
