using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Closetly.DTO;
using Closetly.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Closetly.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] ProductDTO product)
        {
            _productService.CreateProduct(product);

            return Ok();
        }

        [HttpGet("available", Name = "GetAvailableProducts")]
        public IActionResult GetAvailableProducts([FromQuery] ProductFilters filters)
        {
            var products = _productService.GetAvailableProducts(filters);
            return Ok(products);
        }

        [HttpPatch("{id}", Name = "UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] UpdateProductDTO product)
        {
            try
            {
                await _productService.UpdateProduct(id, product);
                return NoContent();

            }
            catch (InvalidOperationException error)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Não Encontrado",
                    Detail = error.Message,
                    Type = "https://httpwg.org/specs/rfc9110.html#status.404"
                });
                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Erro interno do servidor",
                    Detail = ex.Message
                });
            }
        }


        [HttpDelete("{id}", Name = "DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id) 
        {
            try
            {
                await _productService.DeleteProduct(id);
                return NoContent();
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

                if(error.Message.Contains("deletado"))
                {
                    return Conflict(new ProblemDetails
                    {
                        Status = StatusCodes.Status409Conflict,
                        Title = "Conflito",
                        Detail = error.Message,
                        Type = "https://httpwg.org/specs/rfc9110.html#status.409"
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Erro interno do servidor",
                    Detail = ex.Message
                });
            }
        }
    }
}