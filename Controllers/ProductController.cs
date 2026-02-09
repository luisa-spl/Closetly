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
            var updated = await _productService.UpdateProduct(id, product);

            if (!updated)
            {
                return NotFound(new { message = "Produto não encontrado" });
            }

            return NoContent();
        }
    }
}