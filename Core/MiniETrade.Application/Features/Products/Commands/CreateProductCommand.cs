using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniETrade.Application.Abstractions.MessageQue;
using MiniETrade.Application.Repositories;
using MiniETrade.Domain.Entities;
using MiniETrade.Domain.Entities.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.Products.Commands
{
    public class CreateProductCommandRequest : IRequest<CreateProductCommandResponse>
    {
        public string Name { get; set; } = "";
        public int Stock { get; set; }
        public float Price { get; set; }
    }

    public class CreateProductCommandRequestHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IMassTransitService _massTransitService;

        public CreateProductCommandRequestHandler(IProductWriteRepository productWriteRepository, IMassTransitService massTransitService)
        {
            _productWriteRepository = productWriteRepository;
            _massTransitService = massTransitService;
        }

        public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {
            //TODO-HUS geçici olarak kapattık. Açılmalı alttaki de silinmeli.
            //var addedProduct = await _productWriteRepository.AddAsync(new()
            //{
            //    Name = request.Name,
            //    Stock = request.Stock,
            //    Price = request.Price

            //});

            var addedProduct = new Product
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                Name = request.Name,
                Stock = request.Stock,
                Price = request.Price
            };

            var productCreatedEvent = new ProductCreated(addedProduct.Id, addedProduct.CreatedDate, addedProduct.Name, addedProduct.Stock, addedProduct.Price);

            await _massTransitService.Publish(productCreatedEvent); //Ürün ekleme ile ilgili event fırlattık.
            //await _massTransitService.Send(productCreatedEvent); //Ürün ekleme ile ilgili event fırlattık.

            return new CreateProductCommandResponse() { };
        }
    }

    public class CreateProductCommandResponse
    {

    }
}
