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
        public IActionResult GetAvailableProducts()
        {
            var products = _productService.GetAvailableProducts();
            return Ok(products);
        }
    }
}