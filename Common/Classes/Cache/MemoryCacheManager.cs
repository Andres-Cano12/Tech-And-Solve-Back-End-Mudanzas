using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Classes.Cache
{
    public class MemoryCacheManager : IMemoryCacheManager
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ConcurrentDictionary<object, ICacheEntry> _cacheEntries = new ConcurrentDictionary<object, ICacheEntry>();

        public MemoryCacheManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Dispose()
        {
            _memoryCache.Dispose();
        }

        private void PostEvictionCallback(object key, object value, EvictionReason reason, object state)
        {
            _cacheEntries.TryRemove(key, out var _);
        }

        /// <inheritdoc cref="IMemoryCache.TryGetValue"/>
        public bool TryGetValue(object key, out object value)
        {
            return _memoryCache.TryGetValue(key, out value);
        }

        /// <summary>
        ///Crea o sobre-escribe una entrada en la cache y agrega una key al diccionario
        /// </summary>
        /// <param name="key">Objeto que identifica la key</param>
        /// <returns>El nuevo creado <see cref="T:Microsoft.Extensions.Caching.Memory.ICacheEntry" /> instance.</returns>
        public ICacheEntry CreateEntry(object key)
        {
            var entry = _memoryCache.CreateEntry(key);
            entry.RegisterPostEvictionCallback(PostEvictionCallback);
            _cacheEntries.AddOrUpdate(key, entry, (o, cacheEntry) =>
            {
                cacheEntry.Value = entry;
                return cacheEntry;
            });
            return entry;
        }

        /// <inheritdoc cref="IMemoryCache.Remove"/>
        public void Remove(object key)
        {
            _memoryCache.Remove(key);
        }

        /// <inheritdoc cref="IMemoryCacheManager.Clear"/>
        public void Clear()
        {
            foreach (var cacheEntry in _cacheEntries.Keys.ToList())
            {
                _memoryCache.Remove(cacheEntry);
            }
                
        }

        public IEnumerator<KeyValuePair<object, object>> GetEnumerator()
        {
            return _cacheEntries.Select(pair => new KeyValuePair<object, object>(pair.Key, pair.Value.Value)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Obtener todas las keys de todos los items en MemoryCache
        /// </summary>
        public IEnumerator<object> Keys => _cacheEntries.Keys.GetEnumerator();
    }

   
}
