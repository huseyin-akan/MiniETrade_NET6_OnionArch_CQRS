using MediatR;
using MiniETrade.Application.Common.Abstractions.Persistence.Repositories.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.Products.Commands;

public class DeleteProductCommandRequest : IRequest<DeleteProductCommandResponse>
{
    public string ProductId { get; set; }
}

public class DeleteProductCommandRequestHandler : IRequestHandler<DeleteProductCommandRequest, DeleteProductCommandResponse>
{
    private readonly IProductWriteRepository _productWriteRepository;

    public DeleteProductCommandRequestHandler(IProductWriteRepository productWriteRepository)
    {
        _productWriteRepository = productWriteRepository;
    }

    public async Task<DeleteProductCommandResponse> Handle(DeleteProductCommandRequest request, CancellationToken cancellationToken)
    {
        var result = await _productWriteRepository.RemoveAsync(request.ProductId );
        await _productWriteRepository.SaveAsync();
        return new();
    }
}

public class DeleteProductCommandResponse
{
}
