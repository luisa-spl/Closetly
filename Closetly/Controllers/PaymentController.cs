using Closetly.DTO;
using Closetly.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Closetly.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        
        [HttpPost]
        public async Task<IActionResult> PayOrder([FromBody] PaymentDTO payment, CancellationToken cancellationToken)
        {
            try
            {
                await _paymentService.PayOrder(payment, cancellationToken);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Solicitação Inválida",
                    Detail = ex.Message,
                    Type = "https://httpwg.org/specs/rfc9110.html#status.400"
                });
            }
            catch (InvalidOperationException error)
            {
                return Conflict(new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "Conflito",
                    Detail = error.Message,
                    Type = "https://httpwg.org/specs/rfc9110.html#status.409"
                }); 
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Erro interno ao processar pagamento.",
                    Detail = ex.Message
                });
            }
            
            return Ok(new { message = "Pagamento registrado com sucesso." });
        }
    }
}