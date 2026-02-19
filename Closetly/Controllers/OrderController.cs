using Closetly.DTO;
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
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdOrder = await _orderService.CreateOrder(request);
                return StatusCode(201, createdOrder);
            }
            catch (InvalidOperationException error)
            {
                if (error.Message.Contains("Produto com Id") || error.Message.Contains("Usuário com Id"))
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

        [HttpPut("{orderId}/return")]
        public async Task<IActionResult> ReturnOrder(Guid orderId)
        {
            try
            {
                await _orderService.ReturnOrder(orderId);
                return NoContent();
            }
            catch (InvalidOperationException error)
            {
                if (error.Message.Contains("não encontrado"))
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
                });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Erro interno do servidor",
                    Detail = error.Message
                });
            }
        }

        [HttpGet("report/{userId}")]
        public async Task<IActionResult> GetUserOrderReport(Guid userId)
        {
            try
            {
                var report = await _orderService.GetUserOrderReport(userId);
                return Ok(report);
            }
            catch (InvalidOperationException error)
            {
                if (error.Message.Contains("Usuário com Id"))
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
                });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Erro interno do servidor",
                    Detail = error.Message
                });
            }
        }
        [HttpGet("report/{userId}/csv")]
        public async Task<IActionResult> GetUserOrderReportCsv(Guid userId)
        {
            try
            {
                var csv = await _orderService.GetUserOrderReportCsv(userId);
                var bytes = System.Text.Encoding.UTF8.GetBytes(csv);
                var fileName = $"relatorio-pedidos-{userId}-{DateTime.UtcNow:yyyyMMdd}.csv";
                return File(bytes, "text/csv", fileName);
            }
            catch (InvalidOperationException error)
            {
                if (error.Message.Contains("Usuário com Id"))
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
                });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Erro interno do servidor",
                    Detail = error.Message
                });
            }
        }
    }
}
