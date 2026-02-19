using Closetly.DTO;
using Closetly.Services;
using Closetly.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Closetly.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequestDTO request, CancellationToken cancellationToken) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try 
            {
                var createdOrder = await _orderService.CreateOrder(request, cancellationToken);
                return StatusCode(201, createdOrder);
            }
            catch(InvalidOperationException error) 
            {
                if(error.Message.Contains("Produto com Id") || error.Message.Contains("Usuário com Id")) 
                {
                    var problemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status404NotFound,
                        Title = "Não Encontrado",
                        Detail = error.Message,
                        Type = "https://httpwg.org/specs/rfc9110.html#status.404"
                    };
                    return NotFound(problemDetails);
                }

                if (error.Message.Contains("não está disponível"))
                {
                    var problemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status409Conflict,
                        Title = "Conflito",
                        Detail = error.Message,
                        Type = "https://httpwg.org/specs/rfc9110.html#status.409"
                    };
                    return Conflict(problemDetails);
                }

                var badProblemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Solicitação Inválida",
                    Detail = error.Message,
                    Type = "https://httpwg.org/specs/rfc9110.html#status.400"
                };
                return BadRequest(badProblemDetails);
            }
            catch (Exception error)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Erro interno do servidor",
                    Detail = error.Message
                };
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        [HttpPatch("{id}", Name = "CancelOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> CancelOrder([FromRoute] Guid id)
        {
            try
            {
                await _orderService.CancelOrder(id);
                return NoContent();
            }
            catch (InvalidOperationException error)
            {
                if (error.Message.Contains("encontrado"))
                {
                    return NotFound(error.Message);
                }

                return Conflict(error.Message);
            }
            catch (Exception ex)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Erro interno do servidor",
                    Detail = ex.Message
                };
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }
    }
}
