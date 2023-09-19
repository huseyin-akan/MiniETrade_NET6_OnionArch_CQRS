using Microsoft.EntityFrameworkCore;
using MiniETrade.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Common.Abstractions.Persistence.Repositories
{
    public interface IRepository<T>  where T: BaseEntity
    {
        DbSet<T> Table { get; }
    }
}
