using Closetly.DTO;
using Closetly.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Closetly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private IRatingService _ratingService; // _ porque eh privado
        public RatingController(IRatingService rating)
        {
            _ratingService = rating;
        }

        [HttpPost]
        public IActionResult CreateRating([FromBody] RatingDTO rating)
        {
            _ratingService.CreateRating(rating);
            return Ok();
        }
    }
}
