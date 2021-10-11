
namespace Nvk.Cache
{
    /// <summary>
    /// Simple in memory cache.
    /// </summary>
    public class SimpleCacheWithClone : SimpleCache
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SimpleCacheWithClone() : base(true) { }
    }
}
