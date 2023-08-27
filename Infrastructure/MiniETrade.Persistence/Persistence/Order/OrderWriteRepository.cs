using MiniETrade.Application.Repositories;
using MiniETrade.Application.Repositories.Orders;
using MiniETrade.Domain.Entities;
using MiniETrade.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Persistence.Persistence
{    
    public class OrderWriteRepository : WriteRepository<Order>, IOrderWriteRepository
    {
        public OrderWriteRepository(ETicaretAPIDbContext context) : base(context)
        {

        }
    }
}
