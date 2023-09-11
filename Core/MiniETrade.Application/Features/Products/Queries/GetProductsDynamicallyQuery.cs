using MediatR;
using MiniETrade.Application.Common.Abstractions.Persistence.Dynamic;
using MiniETrade.Application.Common.Abstractions.Persistence.Repositories.Products;
using MiniETrade.Application.Repositories;
using MiniETrade.Application.Repositories.Products;
using MiniETrade.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.Products.Queries
{
    public record GetProductsDynamicallyQuery : PageableQueryRequestRec, IRequest<GetProductsDynamicallyQueryResponse>
    {
        public DynamicQuery? DynamicQuery { get; set; }
    }

    public class GetProductsDynamicallyQueryHandler : IRequestHandler<GetProductsDynamicallyQuery, GetProductsDynamicallyQueryResponse>
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductAsyncRepository _productAsyncRepository;
        public GetProductsDynamicallyQueryHandler(IProductReadRepository productReadRepository, IProductAsyncRepository productAsyncRepository)
        {
            _productReadRepository = productReadRepository;
            _productAsyncRepository = productAsyncRepository;
        }
        public async Task<GetProductsDynamicallyQueryResponse> Handle(GetProductsDynamicallyQuery request, CancellationToken cancellationToken)
        {
            Paginate<Product?> products = await _productAsyncRepository.GetListByDynamicAsync(
                dynamic: request.DynamicQuery,
                //include: p => p.Include(m => m.) //TODO-HUS EF eklemek gerek sanırım
                index : request.Page,
                size : request.Size
                );

            var totalCount = _productReadRepository.GetAll(false).Count();
            var result = _productReadRepository.GetAll(false).Select(p => new
            {
                p.Id,
                p.Name,
                p.Stock,
                p.Price,
                p.Created,
                p.LastModified
            }).Skip(request.Page * request.Size).Take(request.Size);

            //TODO-HUS yukarıya Skip ve Take yazılmış ama bunlara gerek yok. Pageable yapısına geçelim.

            return new GetProductsDynamicallyQueryResponse
            {
                TotalCount = totalCount,
                Products = result
            };
        }
    }

    public class GetProductsDynamicallyQueryResponse
    {
        public object Products { get; set; }
        public int TotalCount { get; set; }
    }
}