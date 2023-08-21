using MiniETrade.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Repositories
{
    //TODO-HUS Repositories altındaki klasör isimlendirmelerini çoğul yapalım. Domain class nesneleriyle karışmasın.
    public interface IProductReadRepository :IReadRepository<Product>
    {
    }
}
