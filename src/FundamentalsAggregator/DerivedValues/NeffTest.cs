using System;
using System.Collections.Generic;
using System.Linq;
using log4net;

namespace FundamentalsAggregator.DerivedValues
{
    public interface IDerivedValue
    {
        FundamentalResult Calculate(IEnumerable<ProviderResults> providerResults);
    }

    public class NeffTest : IDerivedValue
    {
        static readonly ILog Log = LogManager.GetLogger(typeof (NeffTest));

        const string Name = "Neff Test";
        const string ToolTip = "(Forecast EPS + Dividend Yield) / PE";

        static readonly FundamentalResult NotEnoughDataResult =
            new FundamentalResult
                {
                    Name = Name,
                    Value = "Unknown (not enough data)",
                    ToolTip = ToolTip
                };

        public FundamentalResult Calculate(IEnumerable<ProviderResults> providerResults)
        {
            if (providerResults == null) throw new ArgumentNullException("providerResults");

            try
            {
                double forecastEps;
                double dividendYield;
                double currentTtmPe;

                if (!TryGet(providerResults, "EPS Estimate Next Year", out forecastEps))
                    return NotEnoughDataResult;

                TryGet(providerResults, new[]
                                            {
                                                "Annual div yield (TTM)",
                                                "Dividend Yield %",
                                                "Dividend Yield"
                                            }, out dividendYield);

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
                               Name = Name,
                               Value = neffTestResult.ToString("0.##"),
                               IsHighlighted = true,
                               ToolTip = String.Format("{0}. Calculated as {1:0.##} + {2:0.##} / {3:0.##}",
                                    ToolTip, forecastEps, dividendYield, currentTtmPe)
                           };

            }
            catch (Exception e)
            {
                Log.Error(e);
                return new FundamentalResult
                {
                    Name = Name,
                    Value = "Unknown (error in calculation)",
                    ToolTip = ToolTip
                };
            }
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
                .Where(r => fundamentalNames.Any(r.NameEquals))
                .FirstOrDefault(r => r.Value.Any(Char.IsDigit));

            if (fundamental == null)
            {
                value = default(double);
                return false;
            }

            return Double.TryParse(fundamental.Value.Replace("%", ""), out value);
        }
    }
}