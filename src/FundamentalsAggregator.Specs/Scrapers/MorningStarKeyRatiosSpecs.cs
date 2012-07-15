using System;
using FundamentalsAggregator.Scrapers;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using SharpTestsEx;

namespace FundamentalsAggregator.Specs.Scrapers
{
    public class MorningStarKeyRatiosSpecs
    {
        [TestFixture]
        public class When_fetching_the_key_ratios
        {
            static ScraperResults results;

            static readonly string[] Symbols = new[] { "AAPL", "GOOG", "XLON:ENRC" };

            [Test, TestCaseSource("Symbols")]
            public void It_should_scrape_fundamentals(string tickerSymbol)
            {
                results = new MorningStarKeyRatios().GetFundamentals(tickerSymbol);
                results.Url.Should().Not.Be.Null();
                results.TickerSymbol.Should().Be(tickerSymbol);
                AssertFundamental<float>(results, "Operating Margin %", Is.GreaterThan(0));
            }

            public static void AssertFundamental<T>(ScraperResults results, string key, Constraint constraint)
            {
                if (!results.Fundamentals.ContainsKey(key))
                    Assert.Fail("Missing fundamental: {0}. Found: {1}", key, 
                        String.Join(", ", results.Fundamentals.Keys));

                var value = Convert.ChangeType(results.Fundamentals[key], typeof (T));

                Assert.That(value, constraint);
            }
        }
    }
}