using System;
using System.Runtime.Serialization;

namespace FundamentalsAggregator.Scrapers
{
    [Serializable]
    public class ScraperException : Exception
    {
        public ScraperException()
        {
        }

        public ScraperException(string message) : base(message)
        {
        }

        public ScraperException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ScraperException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ScraperException(TickerSymbol symbol, IScraper scraper, Exception innerException)
            : this(String.Format("Error looking up symbol {0} from {1}.", symbol, scraper.ProviderName), innerException)
        {
        }
    }
}