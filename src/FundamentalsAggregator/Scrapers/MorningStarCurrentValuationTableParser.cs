using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using log4net;

namespace FundamentalsAggregator.Scrapers
{
    public class MorningStarCurrentValuationTableParser : IMorningStarTableParser
    {
        static readonly ILog Log = LogManager.GetLogger(typeof (MorningStarCurrentValuationTableParser));

        public IDictionary<string, string> Parse(string html)
        {
            var doc = new HtmlDocument
            {
                OptionUseIdAttribute = true,
                OptionFixNestedTags = true
            };

            doc.LoadHtml(html);

            var fundamentals = new Dictionary<string, string>();

            foreach (var row in doc.GetElementbyId("currentValuationTable").Element("tbody").Elements("tr"))
            {
                if (row.Element("th") == null)
                    continue;

                var fundamental = HtmlEntity.DeEntitize(row.Element("th").InnerText);
                var value = HtmlEntity.DeEntitize(row.Elements("td").First().InnerText);

                Log.DebugFormat("Found: {0} = {1}", fundamental, value);
                fundamentals.Add(fundamental, value);
            }

            return fundamentals;
        }
    }
}