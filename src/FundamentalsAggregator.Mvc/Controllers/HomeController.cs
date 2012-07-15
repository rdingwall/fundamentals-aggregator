using System;
using System.Web.Mvc;
using System.Web.UI;

namespace FundamentalsAggregator.Mvc.Controllers
{
    public class HomeController : Controller
    {
        readonly Aggregator aggregator = new Aggregator();

        [OutputCache]
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(Duration = 30 * 60, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "*")]
        public ActionResult Fundamentals(string symbol, string exchange)
        {
            var ts = new TickerSymbol(symbol, (Exchange) Enum.Parse(typeof (Exchange), exchange, true));

            var results = aggregator.Aggregate(ts);

            return View(results);
        }
    }
}
