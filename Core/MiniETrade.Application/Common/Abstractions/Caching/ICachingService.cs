using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Common.Abstractions.Caching
{
    public interface ICachingService
    {
        T? Get<T>(string key);
        void Set<T>(string key, T value);

        //TODO-HUS buraya bir de remove eklicez brochka.
    }
}