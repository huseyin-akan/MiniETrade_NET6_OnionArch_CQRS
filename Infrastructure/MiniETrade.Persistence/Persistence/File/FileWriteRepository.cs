using MiniETrade.Application.Repositories;
using MiniETrade.Application.Repositories.Files;
using MiniETrade.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = MiniETrade.Domain.Entities.File;

namespace MiniETrade.Persistence.Persistence
{
    public class FileWriteRepository : WriteRepository<File>, IFileWriteRepository
    {
        public FileWriteRepository(BaseDbContext context) : base(context)
        {

        }
    }
}
