using MediatR;
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
    public class UpdateProductCommandRequest : IRequest<UpdateProductCommandResponse>, ITransactionalRequest
    {
        public Guid Id{ get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public float Price { get; set; }
    }
    

    public class UpdateProductCommandRequestHandler : IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
    {
        private readonly IProductWriteRepository _productWriteRepository;

        public UpdateProductCommandRequestHandler(IProductWriteRepository productWriteRepository)
        {
            _productWriteRepository = productWriteRepository;
        }

        public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
        {
            var result = _productWriteRepository.Update(new()
            {
                Id = request.Id,
                Name = request.Name,
                Stock = request.Stock,
                Price = request.Price

            });
            await _productWriteRepository.SaveAsync();
            return new UpdateProductCommandResponse() { };
        }        
    }

    public class UpdateProductCommandResponse
    {

    }
}
