using MiniETrade.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Repositories.Products
{
    //TODO-HUS Abstractions --> Persistence --> Repositories kısmındaki repolarım namespacelerini güncellemek gerek.
    public interface IProductReadRepository :IReadRepository<Product>
    {
    }
}
