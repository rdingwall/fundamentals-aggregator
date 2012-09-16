using System;
using System.Linq;
using FundamentalsAggregator.Scrapers;

namespace FundamentalsAggregator
{
    public class Aggregator : IAggregator
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
                               new MorningstarKeyRatios(),
                               //new GoogleFinanceSummary() 
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

            var longName = providerResults
                .Where(r => r.ProviderName == new YahooFinance().ProviderName)
                .SelectMany(r => r.Fundamentals)
                .Where(p => p.Key == "Name")
                .Select(p => p.Value)
                .FirstOrDefault();

            return new AggregationResults(symbol, providerResults, DateTime.UtcNow, longName);
        }
    }
}