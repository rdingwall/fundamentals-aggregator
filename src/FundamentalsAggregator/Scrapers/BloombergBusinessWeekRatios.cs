using System;
using System.Collections.Generic;
using System.Net;
using FundamentalsAggregator.TickerSymbolFormatters;
using HtmlAgilityPack;
using log4net;

namespace FundamentalsAggregator.Scrapers
{
    public class BloombergBusinessweekRatios : IScraper
    {
        static readonly ILog Log = LogManager.GetLogger(typeof (BloombergBusinessweekRatios));

        static readonly ITickerSymbolFormatter Formatter =
            new BloombergBusinessweekFormatter();

        const string UrlFormat = "http://investing.businessweek.com/research/stocks/financials/ratios.asp?ticker={0}";

        public string ProviderName
        {
            get { return "Bloomberg Businessweek"; }
        }

        public ScraperResults GetFundamentals(TickerSymbol symbol)
        {
            if (symbol == null) throw new ArgumentNullException("symbol");

            var formattedSymbol = Formatter.Format(symbol);

            var url = new Uri(String.Format(UrlFormat, formattedSymbol));

            Log.DebugFormat("Looking up {0} from {1}", symbol, url);

            string html;
            using (var webClient = new WebClient())
                html = webClient.DownloadString(url);

            var doc = new HtmlDocument
            {
                OptionFixNestedTags = true
            };

            doc.LoadHtml(html);

            var fundamentals = new Dictionary<string, string>();

            var tds = doc.DocumentNode.SelectNodes("//table[@class='ratioTable']//td");

            foreach (var td in tds)
            {
                if (String.IsNullOrWhiteSpace(td.InnerHtml))
                    continue;

                var name = HtmlEntity.DeEntitize(td.SelectSingleNode("div/p[@class='bold']").InnerText);
                var value = HtmlEntity.DeEntitize(td.SelectSingleNode("div[@class='floatR']/div[1]").InnerText);

                if (value == "--")
                {
                    Log.DebugFormat("Missing: {0} = {1}", name, value);
                    continue;
                }

                Log.DebugFormat("Found: {0} = {1}", name, value);
                fundamentals.Add(name, value);
            }

            return new ScraperResults(url, fundamentals);
        }
    }
}