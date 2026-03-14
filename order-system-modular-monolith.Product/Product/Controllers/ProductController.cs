using Microsoft.AspNetCore.Mvc;
using order_system_modular_monolith.Product.Dtos.UpdateProductDto;
using order_system_modular_monolith.Product.Dtos.CreateProductDto;
using order_system_modular_monolith.Product.Exceptions;
using order_system_modular_monolith.Product.Product.Service;
using System;
using System.Collections.Generic;
using System.Text;
using order_system_modular_monolith.Product.Service;
using Microsoft.AspNetCore.Authorization;

namespace order_system_modular_monolith.Product.Product.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly CreateProductHandler _createProductHandler;
        private readonly UpdateProductHandler _updateProductHandler;


        public ProductController(CreateProductHandler createProductHandler, UpdateProductHandler updateProductHandler)
        {
            _createProductHandler = createProductHandler;
            _updateProductHandler = updateProductHandler;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequestDto input, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _createProductHandler.Handle(input, cancellationToken);
                if (response == null || response.Id == null)
                {
                    return BadRequest("Create Product failed!");
                }
                return Ok("Create Product successfully");
            }
            catch (ExistingException)
            {
                return BadRequest("Product code already exists!");

            }
        }

            [HttpPatch("update")]
            public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductRequestDto input, CancellationToken cancellationToken)
            {
                try
                {
                    var response = await _updateProductHandler.Handle(input, cancellationToken);
                    if (response == null || !response.IsSuccess)
                    {
                        return BadRequest("Create Product failed! " + response?.Error?.Message);
                    }
                    return Ok("Create Product successfully");
                }
                catch (Exception)
                {
                    return BadRequest("Error");
                }

            }
        }
    }
