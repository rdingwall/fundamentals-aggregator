using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace FundamentalsAggregator.Scrapers
{
    public class MorningStarKeyRatios
    {
        // Horrific. This returns an HTML string as JSON.
        const string UrlFormat = "http://financials.morningstar.com/ajax/keystatsAjax.html?t={0}";

        public dynamic Get(string tickerSymbol)
        {
            var url = new Uri(String.Format(UrlFormat, tickerSymbol));

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
                    var value = HtmlEntity.DeEntitize(tr.Elements("td").Last().InnerText);

                    if (value == "—")
                        continue;

                    results.Add(name, value);
                }

                dynamic d = new ExpandoObject();
                d.OperatingMargin = results["Operating Margin %"];
                return d;
            }
        }

    }
}