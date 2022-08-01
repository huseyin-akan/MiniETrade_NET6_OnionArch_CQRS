﻿using MiniETrade.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Repositories
{
    public interface IProductReadRepository :IReadRepository<Product>
    {
    }
}
