using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using log4net;

namespace FundamentalsAggregator.Scrapers
{
    public class MorningStarForwardValuationTableParser : IMorningStarTableParser
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(MorningStarForwardValuationTableParser));

        public IDictionary<string, string> Parse(string html)
        {
            var doc = new HtmlDocument
                          {
                              OptionUseIdAttribute = true,
                              OptionFixNestedTags = true
                          };

            doc.LoadHtml(html);

            var fundamentals = new Dictionary<string, string>();

            // This one is a mess, unclosed tag makes HtmlAgilityPack very confused
            var rows =
                doc.GetElementbyId("forwardValuationTable")
                    .Elements("colgroup").Last()
                    .Elements("colgroup").Last()
                    .Element("tbody")
                    .Elements("tr");

            foreach (var row in rows)
            {
                if (row.Element("th") != null)
                    continue;

                var tds = row.Elements("td");

                var fundamental = HtmlEntity.DeEntitize(tds.ElementAt(0).InnerText);
                if (String.IsNullOrWhiteSpace(fundamental))
                    continue;
                var value = HtmlEntity.DeEntitize(tds.ElementAt(1).InnerText);

                Log.DebugFormat("Found: {0} = {1}", fundamental, value);
                fundamentals.Add(fundamental, value);
            }

            return fundamentals;
        }
    }
}