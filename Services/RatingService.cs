 using Closetly.DTO;
using Closetly.Repository;
using Closetly.Repository.Interface;
using Closetly.Services.Interface;

namespace Closetly.Services
{
    public class RatingService : IRatingService
    {
        private IRatingRepository _repository;
       
        public RatingService(IRatingRepository ratingRepository)
        {
            _repository = ratingRepository;
        }
        public void CreateRating(RatingCreateDTO rating)
        {
            _repository.CreateRating(rating);
        }
    }
}
