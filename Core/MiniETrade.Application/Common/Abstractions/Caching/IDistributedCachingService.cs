using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Common.Abstractions.Caching
{
    public interface IDistributedCachingService : ICachingService
    {
        string GetString(string key);
        void SetString(string key, string value);
    }
}
