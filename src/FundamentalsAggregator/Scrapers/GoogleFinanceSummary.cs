using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using FundamentalsAggregator.TickerSymbolFormatters;
using log4net;

namespace FundamentalsAggregator.Scrapers
{
    public class GoogleFinanceSummary : IScraper
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(GoogleFinanceSummary));
        public string ProviderName { get { return "Google Finance"; } }

        const string XmlApiUrlFormat = "http://www.google.com/ig/api?exchange={0}&stock={1}";
        const string FriendlyUrlFormat = "http://www.google.com/finance?q={0}";
        static readonly ITickerSymbolFormatter Formatter = new GoogleFinanceFormatter();

        public ScraperResults GetFundamentals(TickerSymbol symbol)
        {
            if (symbol == null) throw new ArgumentNullException("symbol");

            var url = new Uri(String.Format(XmlApiUrlFormat, symbol.Exchange, symbol.Symbol));

            IDictionary<string, string> fundamentals;
            try
            {
                fundamentals = GetFundamentals(symbol, url);
            }
            catch (Exception e)
            {
                throw new ScraperException(symbol, this, e);
            }

            if (!fundamentals.Any())
                throw new NoFundamentalsAvailableException();

            var friendlyUrl = new Uri(String.Format(FriendlyUrlFormat, Formatter.Format(symbol)));

            return new ScraperResults(friendlyUrl, fundamentals);
        }

        static IDictionary<string, string> GetFundamentals(TickerSymbol symbol, Uri url)
        {
            Log.DebugFormat("Looking up {0} from {1}", symbol, url);

            string xml;
            using (var webClient = new WebClient())
                xml = webClient.DownloadString(url);

            Log.DebugFormat("Got data ({0} chars)", xml.Length);

            var doc = XDocument.Parse(xml);

            var measures = new[]
                               {
                                   "company",
                                   "currency",
                                   "last",
                                   "high",
                                   "low",
                                   "volume",
                                   "avg_volume",
                                   "market_cap",
                                   "open",
                                   "y_close",
                                   "change",
                                   "perc_change"
                               };

            return doc.Element("xml_api_reply").Element("finance")
                .Elements()
                .Where(e => measures.Contains(e.Name.LocalName))
                .ToDictionary(e => PrettyName(e.Name.LocalName), e => e.Attribute("data").Value);

        }

        static string PrettyName(string measureName)
        {
            measureName = measureName.Replace("_", " ");
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(measureName);
        }
    }
}