using MiniETrade.Application.Features.Products.Queries;
using MiniETrade.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.Products;

//Instead of using AutoMapper we can write our own mapping extensions.
//Pros: You have the full control. No additional library. Works faster. (In a benchmark, manuel mapping worked in 2 microseconds, and automapper worked in 5, a negligable diff really)
//Cons: More codes. Maintenance(If your object model changes, you will have to update here). 

public static class ProductExtensions
{
    public static GetAllProductsResponse MapToDto(this IEnumerable<Product?> products)
    {
        GetAllProductsResponse productsResponse = new();
        List<GetAllProductsDto> mappedProducts = new();

        foreach (var product in products) {
            if (product is null) continue;

            mappedProducts.Add(new GetAllProductsDto
            {
                Created = product.Created,
                Id = product.Id,
                Name = product.Name,
                Price  = product.Price,
                Stock = product.Stock
            });
        }

        productsResponse.Products = mappedProducts;

        return productsResponse;
    }
}