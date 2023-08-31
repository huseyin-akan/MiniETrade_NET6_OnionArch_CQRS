using MediatR;
using MiniETrade.Application.Common.Abstractions.Caching;
using MiniETrade.Application.Common.Abstractions.Transactions;
using MiniETrade.Application.Repositories;
using MiniETrade.Application.Repositories.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.Products.Commands
{
    public class CreateProductCommandRequest : IRequest<CreateProductCommandResponse>, ITransactionalRequest, ICacheRemoverRequest
    {
        public string Name { get; set; } = "";
        public int Stock { get; set; }
        public float Price { get; set; }
        public string? CacheKey => null;
        public bool BypassCache => false;
        public string? CacheGroupKey => "GetProducts";
    }

    public class CreateProductCommandRequestHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
    {
        private readonly IProductWriteRepository _productWriteRepository;
        //private readonly IMassTransitService _massTransitService; TODO-HUS geçici olarak kapattık. Açılmalı

        public CreateProductCommandRequestHandler(
            IProductWriteRepository productWriteRepository
            //,IMassTransitService massTransitService
            )
        {
            _productWriteRepository = productWriteRepository;
            //_massTransitService = massTransitService;
        }

        public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {
            var addedProduct = await _productWriteRepository.AddAsync(new()
            {
                Name = request.Name,
                Stock = request.Stock,
                Price = request.Price

            });

            //var productCreatedEvent = new ProductCreated(addedProduct.Id, addedProduct.CreatedDate, addedProduct.Name, addedProduct.Stock, addedProduct.Price);
            //await _massTransitService.Publish(productCreatedEvent); //Ürün ekleme ile ilgili event fırlattık. TODO-HUS geçici olarak kapattık.

            return new CreateProductCommandResponse() { };
        }
    }

    public class CreateProductCommandResponse
    {

    }
}
