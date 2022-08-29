using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniETrade.Application.Abstractions.Storage;
using MiniETrade.Application.Features.Products.Commands;
using MiniETrade.Application.Features.Products.Queries;
using MiniETrade.Application.Repositories;
using MiniETrade.Domain.Entities;
using System.Diagnostics;
using System.Net;

namespace MiniETrade.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IStorageService _storageService;
        private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

        private readonly IMediator _mediator;

        public ProductsController(IProductReadRepository productReadRepository,
            IProductWriteRepository productWriteRepository,
            IWebHostEnvironment webHostEnvironment,
            IStorageService storageService,
            IProductImageFileWriteRepository productImageFileWriteRepository,
            IMediator mediator)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _webHostEnvironment = webHostEnvironment;
            _storageService = storageService;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _mediator = mediator;
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
        public async Task<IActionResult> GetAllProducts([FromQuery] GetAllProductsQueryRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("addproduct")]
        public async Task<IActionResult> AddProduct([FromBody] CreateProductCommandRequest request)
        {
            var result = await _mediator.Send(request);
            return StatusCode((int)HttpStatusCode.Created);
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
        public async Task<IActionResult> UploadImage(string id)
        {
            var x = Request.Form.Files;

            var result = await _storageService.UploadAsync("product-images", x);

            var product = await _productReadRepository.GetByIdAsync(id);
            
            await _productImageFileWriteRepository.AddRangeAsync(result.Select(r => new ProductImageFile
            {
                FileName = r.fileName,
                Path = r.pathOrContainerName ,
                Products = new List<Product>() { product } 
            }).ToList());

            await _productImageFileWriteRepository.SaveAsync();
            
            return Ok(result);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetProductImages(string id)
        {
            var product = await _productReadRepository.Table.Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
           
            return Ok(product.ProductImages.Select(p => new
            {
                p.Path,
                p.FileName,
                p.Id
            }));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProductImage(string productId, string imageId)
        {
            var product = await _productReadRepository.Table.Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == Guid.Parse(productId));

            var productImageFile = product.ProductImages.FirstOrDefault(p => p.Id == Guid.Parse(imageId));
            product.ProductImages.Remove(productImageFile);
            await _productWriteRepository.SaveAsync();
            return Ok();
        }
    }
}
