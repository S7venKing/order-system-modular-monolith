using Microsoft.AspNetCore.Mvc;
using order_system_modular_monolith.Product.Product.Dtos;
using order_system_modular_monolith.Product.Product.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.Product.Product.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly CreateProductHandler _createProductHandler;

        public ProductController(CreateProductHandler createProductHandler)
        {
            _createProductHandler = createProductHandler;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequestDto input)
        {
            await _createProductHandler.Handle(input, input.cancellationToken);
            return Ok();
        }
    }
}
