using System;
using System.Collections.Generic;

namespace FundamentalsAggregator.Scrapers
{
    public class ScraperResults
    {
        public ScraperResults(Uri url, IDictionary<string, string> fundamentals)
        {
            if (url == null) throw new ArgumentNullException("url");
            if (fundamentals == null) throw new ArgumentNullException("fundamentals");
            Url = url;
            Fundamentals = fundamentals;
        }

        public Uri Url { get; private set; }
        public IDictionary<string, string> Fundamentals { get; private set; }
    }
}