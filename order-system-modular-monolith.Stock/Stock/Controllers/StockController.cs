using Microsoft.AspNetCore.Mvc;
using order_system_modular_monolith.Stock.Dtos.UpdateStockDto;
using order_system_modular_monolith.Stock.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.Stock.Stock.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly UpdateStockHandler _updateStockHandler;


        public StockController(UpdateStockHandler updateStockHandler)
        {
            _updateStockHandler = updateStockHandler;
        }

        [HttpPatch("update")]
        public async Task<IActionResult> UpdateStock([FromBody] UpdateStockRequestDto input, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _updateStockHandler.Handle(input, cancellationToken);
                if (response == null || !response.IsSuccess)
                {
                    return BadRequest("Create Stock failed! " + response?.Error?.Message);
                }
                return Ok("Create Stock successfully");
            }
            catch (Exception)
            {
                return BadRequest("Error");
            }

        }
    }
}
