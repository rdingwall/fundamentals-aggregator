using System;
using System.Runtime.Caching;

namespace FundamentalsAggregator
{
    public class ReadThroughResultCache : IAggregator, IDisposable
    {
        readonly IAggregator source;
        MemoryCache cache;
        readonly TimeSpan expireAfter = TimeSpan.FromHours(3);

        public ReadThroughResultCache(IAggregator source)
        {
            if (source == null) throw new ArgumentNullException("source");
            this.source = source;
            cache = new MemoryCache(typeof(ReadThroughResultCache).Name);
        }

        public AggregationResults Aggregate(TickerSymbol symbol)
        {
            if (symbol == null) throw new ArgumentNullException("symbol");

            var key = symbol.ToString();

            if (cache.Contains(key))
                return (AggregationResults)cache.Get(key);

            var results = source.Aggregate(symbol);

            cache.Add(key, results,
                      new CacheItemPolicy
                          {
                              AbsoluteExpiration = DateTime.UtcNow.Add(expireAfter)
                          });

            return results;
        }
        

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (cache != null)
                    {
                        cache.Dispose();
                        cache = null;
                    }
                }

                disposed = true;
            }
        }

        ~ReadThroughResultCache()
        {
            Dispose(false);
        }

        bool disposed;
    }
}