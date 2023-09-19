using MediatR;
using MiniETrade.Application.Common.Abstractions.Persistence.Repositories.Products;
using MiniETrade.Application.Common.Abstractions.Repositories.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.Products.Queries;

public class GetAllProductsQueryRequest : PageableQueryRequest, IRequest<GetAllProductsQueryResponse>
{       
}

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQueryRequest, GetAllProductsQueryResponse>
{
    private readonly IProductReadRepository _productReadRepository;
    public GetAllProductsQueryHandler(IProductReadRepository productReadRepository)
    {
        _productReadRepository = productReadRepository;
    }
    public async Task<GetAllProductsQueryResponse> Handle(GetAllProductsQueryRequest request, CancellationToken cancellationToken)
    {
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
        
        return new GetAllProductsQueryResponse
        {
            TotalCount = totalCount,
            Products = result
        };
    }
}

public class GetAllProductsQueryResponse
{
    public object Products { get; set; }
    public int TotalCount { get; set; }
}
