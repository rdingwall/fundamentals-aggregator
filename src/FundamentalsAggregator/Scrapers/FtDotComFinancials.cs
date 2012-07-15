using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FundamentalsAggregator.TickerSymbolFormatters;
using HtmlAgilityPack;
using log4net;

namespace FundamentalsAggregator.Scrapers
{
    public class FtDotComFinancials : IScraper
    {
        static readonly ILog Log = LogManager.GetLogger(typeof (FtDotComFinancials));

        static readonly ITickerSymbolFormatter Formatter =
            new FtDotComFormatter();

        const string UrlFormat = "http://markets.ft.com/Research/Markets/Tearsheets/Financials?s={0}";

        public ScraperResults GetFundamentals(TickerSymbol symbol)
        {
            if (symbol == null) throw new ArgumentNullException("symbol");

            var formattedSymbol = Formatter.Format(symbol);

            var url = new Uri(String.Format(UrlFormat, formattedSymbol));

            string html;
            using (var webClient = new WebClient())
                html = webClient.DownloadString(url);

            var doc = new HtmlDocument
            {
                OptionFixNestedTags = true
            };

            doc.LoadHtml(html);

            var fundamentals = new Dictionary<string, string>();
            var trs = doc.DocumentNode.SelectNodes("//div[@class='chartTable']/table/tr");

            foreach (var tr in trs)
            {
                var tds = tr.Elements("td");
                var name = tds.First().InnerText;
                var value = tds.Last().InnerText;

                if (value == "--")
                {
                    Log.DebugFormat("Missing: {0} = {1}", name, value);
                    continue;
                }

                Log.DebugFormat("Found: {0} = {1}", name, value);
                fundamentals.Add(name, value);
            }

            return new ScraperResults(symbol, url, fundamentals, DateTime.UtcNow);
        }
    }
}