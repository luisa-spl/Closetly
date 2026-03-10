using Closetly.DTO;
using Closetly.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Closetly.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private IRatingService _ratingService; // _ porque eh privado
        public RatingController(IRatingService rating)
        {
            _ratingService = rating;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRating([FromBody] RatingCreateDTO rating)
        {
            try
            {
                await _ratingService.CreateRating(rating);

                return Ok();
            }
            catch (InvalidOperationException error) 
            { 
                if (error.Message.Contains("encontrado"))
                {
                    return NotFound(new ProblemDetails
                    {
                        Status = StatusCodes.Status404NotFound,
                        Title = "Não Encontrado",
                        Detail = error.Message,
                        Type = "https://httpwg.org/specs/rfc9110.html#status.404"
                    });
                }

                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Solicitação Inválida",
                    Detail = error.Message,
                    Type = "https://httpwg.org/specs/rfc9110.html#status.400"
                }); ;
            }
        }
    }
}
