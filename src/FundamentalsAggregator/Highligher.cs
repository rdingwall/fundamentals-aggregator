using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FundamentalsAggregator
{
    public class Highligher
    {
        static readonly IEnumerable<Wildcard> Wildcards;

        static Highligher()
        {
            var patterns = new[]
                               {
                                   "*debt*equity*",
                                   "*Forward Price*Earnings*",
                                   "*Forward P/E*",
                                   "*Forward PE*",
                                   "*Current Ratio*",
                                   "P/E (TTM)",
                                   "Price/Sales",
                                   "Price/Earnings"
                               };

            Wildcards = patterns.Select(s => new Wildcard(s, RegexOptions.IgnoreCase)).ToList();
        }

        public bool IsHighlighted(string fundamental)
        {
            return Wildcards.Any(w => w.IsMatch(fundamental));
        }
    }
}