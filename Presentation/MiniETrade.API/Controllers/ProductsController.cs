using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniETrade.Application.Repositories;
using MiniETrade.Application.RequestParameters;
using MiniETrade.Application.Services;
using MiniETrade.Domain.Entities;
using System.Diagnostics;

namespace MiniETrade.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;

        public ProductsController(IProductReadRepository productReadRepository,
            IProductWriteRepository productWriteRepository,
            IWebHostEnvironment webHostEnvironment,
            IFileService fileService)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
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
        public IActionResult GetAllProducts([FromQuery] Pagination pagination)
        {
            var totalCount = _productReadRepository.GetAll(false).Count();
            var result = _productReadRepository.GetAll(false).Select(p => new
            {
                p.Id,
                p.Name,
                p.Stock,
                p.Price,
                p.CreatedDate,
                p.UpdatedDate
            }).Skip(pagination.Page * pagination.Size).Take(pagination.Size);
            return Ok(new { totalCount, result });
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

        [HttpPost("uploadimage")]
        public async Task<IActionResult> UploadImage()
        {
            var x = Request.Form.Files;
            Console.WriteLine("İş başladı");
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var result = await _fileService.UploadAsync("product-images", x);            

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Geçen süre ise : " + elapsedMs );

            return Ok(result);
        }
    }
}
