using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using log4net;

namespace FundamentalsAggregator.Scrapers
{
    public class MorningStarFundamentalsTableParser
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(MorningStarFundamentalsTableParser));
        readonly string html;
        readonly HtmlDocument doc;
        readonly Dictionary<string, IEnumerable<Tuple<string, string>>> fundamentals;

        public MorningStarFundamentalsTableParser(string html)
        {
            this.html = html;
            doc = new HtmlDocument
            {
                OptionUseIdAttribute = true,
                OptionFixNestedTags = true
            };

            doc.LoadHtml(html);

            var financials = doc.GetElementbyId("financials").Element("table");

            fundamentals =
                new Dictionary<string, IEnumerable<Tuple<string, string>>>();

            var periodNames = financials.Element("thead").Element("tr").Elements("th")
                .Select(e => e.InnerText).ToList();

            foreach (var tr in financials.Element("tbody").Elements("tr"))
            {
                var th = tr.Element("th");
                if (th == null)
                    continue;

                var fundamental = HtmlEntity.DeEntitize(th.InnerText);

                var values = new List<Tuple<string, string>>();

                var tds = tr.Elements("td").ToList();
                for (int i = 0; i < tds.Count(); i++)
                {
                    var td = tds[i];
                    var value = HtmlEntity.DeEntitize(td.InnerText);
                    if (value == "—") // &mdash;
                        continue;

                    var periodName = periodNames.ElementAt(i + 1);
                    values.Add(Tuple.Create(periodName, value));
                }

                fundamentals.Add(fundamental, values);
            }

            PrintToLog();
        }

        void PrintToLog()
        {
            Log.Debug("Parsed:");
            foreach (var fundamental in fundamentals.Keys)
            {
                var values = fundamentals[fundamental].Select(p => String.Format("{0}={1}", p.Item1, p.Item2));
                Log.DebugFormat("{0}: {1}", fundamental, String.Join(", ", values));
            }
        }

        public IEnumerable<string> FundamentalNames { get { return fundamentals.Keys; } }

        public string GetLatestValue(string name)
        {
            var pair = fundamentals[name].OrderByDescending(t => t.Item1).FirstOrDefault();

            return pair == null ? null : pair.Item2;
        }
    }
}