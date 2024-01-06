using kafka.ProductApi.Models;
using kafka.ProductApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace kafka.ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly ProducerService _producerService;
        private readonly IConfiguration _configuration;

        public InventoryController(ProducerService producerService, IConfiguration configuration)
        {
            _producerService = producerService;
            _configuration = configuration;
        }


        [HttpPost]
        public async Task<IActionResult> InventoryStcok([FromBody] InventoryStockRequest request)
        {
            var message = JsonSerializer.Serialize(request);
            var topic = _configuration["Kafka:Topic"];
            await _producerService.ProduceAsync(topic, message);

            return Ok("Inventory Updated Successfully...");
        }
    }
}
