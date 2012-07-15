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
            static ScraperResults results;

            Because of = () => results = new MorningStarKeyRatios().GetFundamentals("AAPL");

            It should_include_the_ticker_symbol = () => results.TickerSymbol.ShouldEqual("AAPL");

            It should_include_the_url_to_visit = () => results.Url.ShouldNotBeNull();

            It should_return_some_fundamentals = () => results.Fundamentals.ShouldNotBeEmpty();

            It should_return_the_operating_margin =
                () => Convert.ToSingle(results.Fundamentals["Operating Margin %"]).ShouldBeGreaterThan(0);
        }
    }
}