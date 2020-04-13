using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Common.Classes.Cache
{
    public interface IMemoryCacheManager: IEnumerable<KeyValuePair<object, object>>, IMemoryCache
    {
        void Clear();
    }
}
