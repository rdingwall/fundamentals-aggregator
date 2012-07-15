using System;
using System.Linq;
using FundamentalsAggregator.Scrapers;

namespace FundamentalsAggregator
{
    public class Aggregator
    {
        readonly ScraperRunner[] scrapers;

        public Aggregator()
        {
            scrapers = new IScraper[]
                           {
                               new FtDotComFinancials(),
                               new FtDotComSummary(),
                               new BloombergBusinessweekRatios(),
                               new YahooFinance(),
                               new MorningstarKeyRatios()
                           }.Select(s => new ScraperRunner(s))
                           .ToArray();
        }

        public AggregationResults Aggregate(TickerSymbol symbol)
        {
            if (symbol == null) throw new ArgumentNullException("symbol");

            var providerResults = scrapers
                .AsParallel()
                .Select(s => s.GetFundamentals(symbol))
                .OrderBy(r => r.ProviderName)
                .ToList();

            return new AggregationResults(symbol, providerResults, DateTime.UtcNow);
        }
    }
}