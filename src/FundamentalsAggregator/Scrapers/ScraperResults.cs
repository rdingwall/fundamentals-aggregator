using System;
using System.Collections.Generic;

namespace FundamentalsAggregator.Scrapers
{
    public class ScraperResults
    {
        public ScraperResults()
        {
            Fundamentals = new Dictionary<string, string>();
        }

        public string TickerSymbol { get; set; }
        public Uri Url { get; set; }
        public IDictionary<string, string> Fundamentals { get; set; }
        public DateTime Timestamp { get; set; }
    }
}