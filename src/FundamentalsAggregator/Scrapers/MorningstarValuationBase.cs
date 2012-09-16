using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FundamentalsAggregator.TickerSymbolFormatters;
using log4net;

namespace FundamentalsAggregator.Scrapers
{
    public abstract class MorningstarValuationBase : IScraper
    {
        readonly string ajaxUrlFormat;
        readonly string viewUrlFormat;
        readonly IMorningStarTableParser parser;
        static readonly MorningStarFormatter Formatter = new MorningStarFormatter();
        readonly ILog log;

        protected MorningstarValuationBase(string ajaxUrlFormat, string viewUrlFormat, IMorningStarTableParser parser)
        {
            if (ajaxUrlFormat == null) throw new ArgumentNullException("ajaxUrlFormat");
            if (viewUrlFormat == null) throw new ArgumentNullException("viewUrlFormat");
            if (parser == null) throw new ArgumentNullException("parser");
            log = LogManager.GetLogger(GetType());
            this.ajaxUrlFormat = ajaxUrlFormat;
            this.viewUrlFormat = viewUrlFormat;
            this.parser = parser;
        }

        public abstract string ProviderName { get; }

        public ScraperResults GetFundamentals(TickerSymbol symbol)
        {
            log.DebugFormat("Looking up symbol {0}", symbol);

            var symbolFormat = Formatter.Format(symbol);
            var region = Formatter.GetRegionOrDefault(symbol);
            var strUrl = String.Format(ajaxUrlFormat, symbolFormat);
            if (region != null)
                strUrl = String.Format("{0}&region={1}", strUrl, region);

            var url = new Uri(strUrl);

            log.DebugFormat("Using URL = {0}", strUrl);

            IDictionary<string, string> fundamentals;
            try
            {
                fundamentals = ScrapeFundamentals(url);
            }
            catch (Exception e)
            {
                throw new ScraperException(symbol, this, e);
            }

            if (!fundamentals.Any())
                throw new NoFundamentalsAvailableException();

            var friendlyUrl = new Uri(String.Format(viewUrlFormat, symbol.Symbol));
            return new ScraperResults(friendlyUrl, fundamentals);
        }

        IDictionary<string, string> ScrapeFundamentals(Uri url)
        {
            using (var client = new WebClient())
            {
                var html = client.DownloadString(url);

                log.DebugFormat("Got response body ({0} chars)", html.Length);

                if (String.IsNullOrWhiteSpace(html))
                    return new Dictionary<string, string>();

                var fundamentals = parser.Parse(html);

                if (!fundamentals.Any())
                    throw new Exception("All fundamentals were missing?");

                return fundamentals;
            }
        }
    }
}