using Microsoft.EntityFrameworkCore.Query;
using MiniETrade.Application.Common.Abstractions.Persistence.Dynamic;
using MiniETrade.Application.Common.Abstractions.Persistence.Repositories.Products;
using MiniETrade.Application.Repositories;
using MiniETrade.Domain.Entities;
using MiniETrade.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Persistence.Persistence.Products
{
    public class ProductAsyncRepository : EfAsyncRepository<Product, BaseDbContext>,  IProductAsyncRepository
    {
        public ProductAsyncRepository(BaseDbContext context) : base(context)
        {

        }
    }
}
