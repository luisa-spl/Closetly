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
               
                return NotFound(error.Message);
                
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