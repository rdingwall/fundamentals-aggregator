using System;
using System.Diagnostics;
using System.Linq;
using FundamentalsAggregator.Scrapers;
using log4net;

namespace FundamentalsAggregator
{
    public class ScraperRunner
    {
        static readonly ILog Log = LogManager.GetLogger(typeof (ScraperRunner));
        readonly IScraper scraper;
        readonly Highligher highlighter;

        public ScraperRunner(IScraper scraper)
        {
            if (scraper == null) throw new ArgumentNullException("scraper");
            this.scraper = scraper;
            highlighter = new Highligher();
        }

        public ProviderResults GetFundamentals(TickerSymbol symbol)
        {
            if (symbol == null) throw new ArgumentNullException("symbol");

            var sw = Stopwatch.StartNew();

            try
            {
                Log.DebugFormat("Looking up {0} via {1}", symbol, scraper.GetType());
                var results = scraper.GetFundamentals(symbol);

                var fundamentals = results.Fundamentals.Select(p => new FundamentalResult
                                                     {
                                                         Name = p.Key,
                                                         Value = p.Value,
                                                         IsHighlighted = highlighter.IsHighlighted(p.Key)
                                                     }).ToList();

                return new ProviderResults(scraper.ProviderName, results.Url, fundamentals);
            }
            catch (NoFundamentalsAvailableException e)
            {
                Log.ErrorFormat("Could not find any fundamentals from {0}.", scraper.GetType());
                Log.Error(e);

                return new ProviderResults(scraper.ProviderName);
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