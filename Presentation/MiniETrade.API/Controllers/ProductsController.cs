using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniETrade.Application.Repositories;
using MiniETrade.Domain.Entities;

namespace MiniETrade.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;

        public ProductsController(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
        }

        [HttpGet("addsome")]
        public async Task<IActionResult> AddProducts()
        {
            await _productWriteRepository.AddRangeAsync(
                new()
                {
                    new() { Id = Guid.NewGuid(), Name = "Product1", Price = 100, CreatedDate = DateTime.UtcNow, Stock = 10 },
                    new() { Id = Guid.NewGuid(), Name = "Product2", Price = 200, CreatedDate = DateTime.UtcNow, Stock = 20 },
                    new() { Id = Guid.NewGuid(), Name = "Product3", Price = 300, CreatedDate = DateTime.UtcNow, Stock = 30 },
                    new() { Id = Guid.NewGuid(), Name = "Product4", Price = 400, CreatedDate = DateTime.UtcNow, Stock = 40 },
                    new() { Id = Guid.NewGuid(), Name = "Product5", Price = 500, CreatedDate = DateTime.UtcNow, Stock = 50 }
                }
                );
            await _productWriteRepository.SaveAsync();
            return Ok("Ekleme işlemi başarılı oldu");
        }

        [HttpGet("getall")]
        public IActionResult GetAllProducts()
        {
            var result = _productReadRepository.GetAll().ToList();
            return Ok(result);
        }

        [HttpPost("addproduct")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {

            if (ModelState.IsValid)
            {

            }

            var result = await _productWriteRepository.AddAsync(product);
            await _productWriteRepository.SaveAsync();
            return Ok(result);
        }

        [HttpDelete("deleteproduct/{productId}")]
        public async Task<IActionResult> DeleteProduct( [FromRoute]string productId)
        {
            var result = await _productWriteRepository.RemoveAsync(productId);
            await _productWriteRepository.SaveAsync();
            return Ok(result);
        }

        [HttpPut("updateproduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            var result = _productWriteRepository.Update(product);
            await _productWriteRepository.SaveAsync();
            return Ok(result);
        }
    }
}
