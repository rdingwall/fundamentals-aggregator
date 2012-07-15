using System;
using System.Collections.Generic;
using System.Net;
using FundamentalsAggregator.TickerSymbolFormatters;
using log4net;
using Newtonsoft.Json;

namespace FundamentalsAggregator.Scrapers
{
    public class MorningstarKeyRatios : IScraper
    {
        static readonly ILog Log = LogManager.GetLogger(typeof (MorningstarKeyRatios));
        static readonly ITickerSymbolFormatter Formatter = new MorningStarFormatter();

        // Horrific. This returns an HTML string as JSON.
        const string AjaxUrlFormat = "http://financials.morningstar.com/ajax/keystatsAjax.html?t={0}";
        const string ViewUrlFormat = "http://financials.morningstar.com/ratios/r.html?t={0}";

        public string ProviderName
        {
            get { return "Morningstar"; }
        }

        public ScraperResults GetFundamentals(TickerSymbol symbol)
        {
            Log.DebugFormat("Looking up symbol {0}", symbol);

            var symbolFormat = Formatter.Format(symbol);
            var url = new Uri(String.Format(AjaxUrlFormat, symbolFormat));

            Log.DebugFormat("Using URL = {0}", url);

            try
            {
                var fundamentals = ScrapeFundamentals(url);
                var friendlyUrl = new Uri(String.Format(ViewUrlFormat, symbol.Symbol));
                return new ScraperResults(friendlyUrl, fundamentals);
            }
            catch (Exception e)
            {
                throw new ScraperException(String.Format("Error scraping MorningStar Key Ratios from {0}.", url), e);
            }
        }

        static IDictionary<string, string> ScrapeFundamentals(Uri url)
        {
            using (var client = new WebClient())
            {
                var json = client.DownloadString(url);

                Log.DebugFormat("Got response body ({0} chars)", json.Length);

                var o = JsonConvert.DeserializeAnonymousType<dynamic>(json, new {});

                var html = (string) o.ksContent.Value;

                var parser = new MorningStarFundamentalsTableParser(html);

                var results = new Dictionary<string, string>();

                foreach (var fundamental in parser.FundamentalNames)
                {
                    var value = parser.GetLatestValue(fundamental);
                    if (value == null)
                    {
                        Log.DebugFormat("Missing: {0} = {1}", fundamental, value);
                        continue;
                    }
                    results.Add(fundamental, value);
                    Log.DebugFormat("Found: {0} = {1}", fundamental, value);
                }

                return results;
            }
        }
    }
}