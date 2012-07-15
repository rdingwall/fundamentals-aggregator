using System;
using System.Diagnostics;
using FundamentalsAggregator.Scrapers;
using log4net;

namespace FundamentalsAggregator
{
    public class ScraperRunner
    {
        static readonly ILog Log = LogManager.GetLogger(typeof (ScraperRunner));
        readonly IScraper scraper;

        public ScraperRunner(IScraper scraper)
        {
            if (scraper == null) throw new ArgumentNullException("scraper");
            this.scraper = scraper;
        }

        public ProviderResults GetFundamentals(TickerSymbol symbol)
        {
            if (symbol == null) throw new ArgumentNullException("symbol");

            var sw = Stopwatch.StartNew();

            try
            {
                Log.DebugFormat("Looking up {0} via {1}", symbol, scraper.GetType());
                var results = scraper.GetFundamentals(symbol);

                return new ProviderResults(scraper.ProviderName, results.Url, results.Fundamentals);
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Scraper {0} failed!", scraper.GetType());
                Log.Error(e);

                return new ProviderResults(scraper.ProviderName, e);
            }
            finally
            {
                sw.Stop();
                Log.DebugFormat("{0} total duration: {1}", scraper.GetType(), sw.Elapsed);
            }
        }
    }
}