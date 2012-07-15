using System;
using FundamentalsAggregator.Scrapers;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using SharpTestsEx;

namespace FundamentalsAggregator.Specs.Scrapers
{
    public class MorningStarKeyRatiosTests
    {
        [TestFixture]
        public class When_fetching_the_key_ratios
        {
            static ScraperResults results;

            static readonly TickerSymbol[] Symbols = new[]
                                                         {
                                                             new TickerSymbol("AAPL", Exchange.Nasdaq), 
                                                             new TickerSymbol("GOOG", Exchange.Nasdaq), 
                                                             new TickerSymbol("ENRC", Exchange.Lse),
                                                             new TickerSymbol("GXY", Exchange.Asx),
                                                         };

            [Test, TestCaseSource("Symbols")]
            public void It_should_scrape_fundamentals(TickerSymbol tickerSymbol)
            {
                results = new MorningStarKeyRatios().GetFundamentals(tickerSymbol);
                results.Url.Should().Not.Be.Null();
                results.TickerSymbol.Should().Be(tickerSymbol);
                AssertFundamental<float>(results, "Operating Margin %", Is.Not.EqualTo(0));
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