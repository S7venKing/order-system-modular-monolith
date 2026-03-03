using Microsoft.AspNetCore.Mvc;
using order_system_modular_monolith.Product.Product.Dtos;
using order_system_modular_monolith.Product.Product.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.Product.Product.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly CreateProductHandler _createProductHandler;

        public ProductController(CreateProductHandler createProductHandler)
        {
            _createProductHandler = createProductHandler;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequestDto input, CancellationToken cancellationToken)
        {
            var response = await _createProductHandler.Handle(input, cancellationToken);
            if (response == null || response.Id == null)
            {
                return BadRequest("Create Product failed!");
            }
            return Ok("Create Product successfully");
        }
    }
}
