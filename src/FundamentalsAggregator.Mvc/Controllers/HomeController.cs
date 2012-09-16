using System;
using System.Web.Mvc;
using System.Web.UI;
using System.Linq;

namespace FundamentalsAggregator.Mvc.Controllers
{
    public class HomeController : Controller
    {
        readonly IAggregator aggregator;

        public HomeController(IAggregator aggregator)
        {
            if (aggregator == null) throw new ArgumentNullException("aggregator");
            this.aggregator = aggregator;
        }

        [OutputCache(Duration = 60 * 60, VaryByParam = "none")]
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(Duration = 30 * 60, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "*")]
        public ActionResult Fundamentals(string symbol, string exchange, 
            [ModelBinder(typeof(TruthyBooleanModelBinder))] bool json = false)
        {
            var ts = new TickerSymbol(symbol, (Exchange) Enum.Parse(typeof (Exchange), exchange, true));

            var results = aggregator.Aggregate(ts);

            if (Request.AcceptTypes.Contains("application/json") || json)
                return new JsonNetResult {Data = results};

            return View(results);
        }
    }
}
