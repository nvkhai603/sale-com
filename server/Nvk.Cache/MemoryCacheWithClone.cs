using Microsoft.Extensions.Caching.Memory;

namespace Nvk.Cache
{
    /// <summary>
    /// Simple in memory cache.
    /// </summary>
    public class MemoryCacheWithClone : MemoryCache
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="cache">The currently configured cache</param>
        public MemoryCacheWithClone(IMemoryCache cache) : base(cache, true) { }
    }
}
