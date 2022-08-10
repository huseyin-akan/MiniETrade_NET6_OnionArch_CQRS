using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = MiniETrade.Domain.Entities.File;

namespace MiniETrade.Application.Repositories
{
    public interface IFileWriteRepository : IWriteRepository<File>
    {
    }
}
