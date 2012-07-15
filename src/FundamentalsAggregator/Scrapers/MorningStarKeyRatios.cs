using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FundamentalsAggregator.TickerSymbolFormatters;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace FundamentalsAggregator.Scrapers
{
    public class MorningStarKeyRatios : IScraper
    {
        static readonly ITickerSymbolFormatter Formatter = new MorningStarTickerSymbolFormatter();

        // Horrific. This returns an HTML string as JSON.
        const string AjaxUrlFormat = "http://financials.morningstar.com/ajax/keystatsAjax.html?t={0}";
        const string ViewUrlFormat = "http://financials.morningstar.com/ratios/r.html?t={0}";

        public ScraperResults GetFundamentals(TickerSymbol symbol)
        {
            var symbolFormat = Formatter.Format(symbol);
            var url = new Uri(String.Format(AjaxUrlFormat, symbolFormat));

            try
            {
                var fundamentals = ScrapeFundamentals(url);
                var friendlyUrl = new Uri(String.Format(ViewUrlFormat, symbol.Symbol));
                return new ScraperResults(symbol, friendlyUrl, fundamentals, DateTime.UtcNow);
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

                var o = JsonConvert.DeserializeAnonymousType<dynamic>(json, new {});

                var doc = new HtmlDocument
                              {
                                  OptionUseIdAttribute = true,
                                  OptionFixNestedTags = true
                              };

                doc.LoadHtml((string)o.ksContent.Value);

                var results = new Dictionary<string, string>();

                var financials = doc.GetElementbyId("financials").Element("table").Element("tbody");

                foreach (var tr in financials.Elements("tr"))
                {
                    var th = tr.Element("th");
                    if (th == null)
                        continue;

                    var name = HtmlEntity.DeEntitize(th.InnerText);

                    // Use the latest TTM (furthest left) value.
                    var value = HtmlEntity.DeEntitize(tr.Elements("td").Last().InnerText);

                    if (value == "—")
                        continue;

                    results.Add(name, value);
                }

                return results;
            }
        }
    }
}