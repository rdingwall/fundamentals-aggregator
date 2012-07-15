using System;
using FundamentalsAggregator.Scrapers;
using Machine.Specifications;

namespace FundamentalsAggregator.Specs.Scrapers
{
    [Subject(typeof(MorningStarKeyRatios))]
    public class MorningStarKeyRatiosSpecs
    {
        public class When_fetching_the_key_ratios
        {
            static dynamic results;

            Because of = () => results = new MorningStarKeyRatios().Get("AAPL");

            It should_return_the_operating_margin =
                () =>
                {
                    float value = Convert.ToSingle(results.OperatingMargin);
                    value.ShouldBeGreaterThan(0);
                };
        }
    }
}