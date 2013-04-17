using System;
using System.Collections.Generic;
using System.Linq;

namespace FundamentalsAggregator.DerivedValues
{
    public interface IDerivedValue
    {
        FundamentalResult Calculate(IEnumerable<ProviderResults> providerResults);
    }

    public class NeffTest : IDerivedValue
    {
        static readonly FundamentalResult NotEnoughDataResult =
            new FundamentalResult
                {
                    Name = "Neff Test",
                    Value = "Unknown (not enough data)"
                };

        public FundamentalResult Calculate(IEnumerable<ProviderResults> providerResults)
        {
            double forecastEps;
            double dividendYield;
            double currentTtmPe;

            if (!TryGet(providerResults, "EPS Estimate Next Year", out forecastEps))
                return NotEnoughDataResult;

            if (!TryGet(providerResults, new[]
                                             {
                                                 "Annual div yield (TTM)", 
                                                 "Dividend Yield %", 
                                                 "Dividend Yield"
                                             }, out dividendYield))
                return NotEnoughDataResult;

            if (!TryGet(providerResults, new[]
                                             {
                                                 "P/E (TTM)", 
                                                 "P/E Ratio",
                                                 "Price/Earnings"
                                             }, out currentTtmPe))
                return NotEnoughDataResult;

            var neffTestResult = (forecastEps + dividendYield) / currentTtmPe;

            return new FundamentalResult
                       {
                           Name = "Neff Test",
                           Value = neffTestResult.ToString("0.##"),
                           IsHighlighted = true,
                           ToolTip = String.Format("(Forecast EPS + Dividend Yield) / PE. Calculated as {0:0.##} + {1:0.##} / {2:0.##}",
                                forecastEps, dividendYield, currentTtmPe)
                       };
        }

        static bool TryGet(IEnumerable<ProviderResults> providerResults,
                           string fundamentalName, out double value)
        {
            var fundamental = providerResults
                .SelectMany(p => p.Fundamentals)
                .FirstOrDefault(r => r.NameEquals(fundamentalName));

            if (fundamental == null)
            {
                value = default(double);
                return false;
            }

            return Double.TryParse(fundamental.Value.Replace("%", ""), out value);
        }

        static bool TryGet(IEnumerable<ProviderResults> providerResults,
                           IEnumerable<string> fundamentalNames, out double value)
        {
            var fundamental = providerResults
                .SelectMany(p => p.Fundamentals)
                .FirstOrDefault(r => fundamentalNames.Any(r.NameEquals));

            if (fundamental == null)
            {
                value = default(double);
                return false;
            }

            return Double.TryParse(fundamental.Value.Replace("%", ""), out value);
        }
    }
}