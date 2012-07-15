using System;
using System.Collections.Generic;

namespace FundamentalsAggregator.Scrapers
{
    public class ScraperResults
    {
        public ScraperResults(TickerSymbol tickerSymbol, Uri url, 
            IDictionary<string, string> fundamentals, DateTime timestamp)
        {
            if (tickerSymbol == null) throw new ArgumentNullException("tickerSymbol");
            if (url == null) throw new ArgumentNullException("url");
            if (fundamentals == null) throw new ArgumentNullException("fundamentals");
            TickerSymbol = tickerSymbol;
            Url = url;
            Fundamentals = fundamentals;
            Timestamp = timestamp;
        }

        public TickerSymbol TickerSymbol { get; private set; }
        public Uri Url { get; private set; }
        public IDictionary<string, string> Fundamentals { get; private set; }
        public DateTime Timestamp { get; private set; }
    }
}