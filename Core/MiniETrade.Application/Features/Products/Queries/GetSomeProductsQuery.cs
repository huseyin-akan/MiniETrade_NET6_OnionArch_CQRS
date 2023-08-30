using MediatR;
using MiniETrade.Application.Common.Abstractions;
using MiniETrade.Application.Common.Abstractions.Caching;
using MiniETrade.Application.Repositories;
using MiniETrade.Application.Repositories.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.Products.Queries
{
    //TODO-HUS buraya Include yaptığımız bir query yazıcaz.
    public record GetSomeProductsQuery : PageableQueryRequestRec, IRequest<GetSomeProductsQueryResponse>, ICachableRequest
    {
        public string CacheKey => $"GetSomeProductQuery({Page},{Size})"; //It's important this value is unique

        public bool BypassCache => false; //If true response wont be brought from cache.

        public string? CacheGroupKey => throw new NotImplementedException();
    }

    public class GetSomeProductsQueryHandler : IRequestHandler<GetSomeProductsQuery, GetSomeProductsQueryResponse>
    {
        private readonly IProductReadRepository _productReadRepository;
        public GetSomeProductsQueryHandler(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }
        public async Task<GetSomeProductsQueryResponse> Handle(GetSomeProductsQuery request, CancellationToken cancellationToken)
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
            }).Skip(request.Page * request.Size).Take(request.Size);

            //TODO-HUS yukarıya Skip ve Take yazılmış ama bunlara gerek yok. Pageable yapısına geçelim.

            return new GetSomeProductsQueryResponse
            {
                TotalCount = totalCount,
                Products = result
            };
        }
    }

    public class GetSomeProductsQueryResponse
    {
        public object Products { get; set; }
        public int TotalCount { get; set; }
    }

    //TODO-HUS yazılmış bir PageableQueryRequest abstract class var. Onun yerine bu recorda geçicez muhtemelen.
    public record PageableQueryRequestRec
    {
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 5;
    }
}