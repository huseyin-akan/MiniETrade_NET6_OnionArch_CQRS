using Application.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using MiniETrade.Application.Repositories.Products;
using MiniETrade.Domain.Entities;
using MiniETrade.Persistence.Contexts;
using MiniETrade.Persistence.Persistence.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.Common.Abstractions.Repositories;

public class ProdutWriteRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;
    private readonly IProductWriteRepository _productWriteRepository;

    public ProdutWriteRepositoryTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _productWriteRepository = new ProductWriteRepository(_fixture.AppDbContext);
    }

    [Fact]
    public async Task AddProductToDatabase()
    {
        Product productToAdd = new()
        {
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.Now,
            Name = "Mouse",
            Price = 12,
            Stock = 33
        };

        var result = await _productWriteRepository.AddAsync(productToAdd);

        result.Should().NotBeNull();
        result.Should().BeOfType<Product>();    
    }
}