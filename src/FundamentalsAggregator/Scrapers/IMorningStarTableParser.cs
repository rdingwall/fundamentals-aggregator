using System.Collections.Generic;

namespace FundamentalsAggregator.Scrapers
{
    public interface IMorningStarTableParser
    {
        IDictionary<string, string> Parse(string html);
    }
}