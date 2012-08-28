using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using FundamentalsAggregator.TickerSymbolFormatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using log4net;
using System.Xml.XPath;

namespace FundamentalsAggregator.Scrapers
{
    public class GoogleFinanceSummary : IScraper
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(GoogleFinanceSummary));
        public string ProviderName { get { return "Google Finance"; } }

        // http://developer.yahoo.com/yql/console/?q=show%20tables&env=store://datatables.org/alltableswithkeys#h=select%20%2a%20from%20google.igoogle.stock%20where%20stock%3D%27ibm%27%3B
        const string XmlApiUrlFormat = "http://query.yahooapis.com/v1/public/yql?q=select * from google.igoogle.stock where stock='{0}';&env=store://datatables.org/alltableswithkeys&format=json";
        const string FriendlyUrlFormat = "http://www.google.com/finance?q={0}";
        static readonly ITickerSymbolFormatter Formatter = new GoogleFinanceFormatter();

        public ScraperResults GetFundamentals(TickerSymbol symbol)
        {
            if (symbol == null) throw new ArgumentNullException("symbol");

            var formattedSymbol = Formatter.Format(symbol);

            var url = new Uri(String.Format(XmlApiUrlFormat, formattedSymbol));

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

            var friendlyUrl = new Uri(String.Format(FriendlyUrlFormat, formattedSymbol));

            return new ScraperResults(friendlyUrl, fundamentals);
        }

        static IDictionary<string, string> GetFundamentals(TickerSymbol symbol, Uri url)
        {
            Log.DebugFormat("Looking up {0} from {1}", symbol, url);

            string json;
            using (var webClient = new WebClient())
                json = webClient.DownloadString(url);

            Log.DebugFormat("Got data ({0} chars)", json.Length);

            var doc = JsonConvert.DeserializeObject<dynamic>(json);

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
                                   "perc_change",
                                   "exchange"
                               };

            var finance = (JContainer)doc.query.results.xml_api_reply.finance;

            return finance.Children()
                .OfType<JProperty>()
                .Where(p => measures.Contains(p.Name))
                .ToDictionary(p => PrettyName(p.Name), p => GetValue(p.Value));
        }

        static string GetValue(JToken value)
        {
            if (value is JValue)
                return value.ToString();

            return value["data"].ToString();
        }

        static string PrettyName(string measureName)
        {
            measureName = measureName.Replace("_", " ");
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(measureName);
        }
    }
}