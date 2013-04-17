using FundamentalsAggregator.Scrapers;
using NUnit.Framework;
using SharpTestsEx;

namespace FundamentalsAggregator.Specs.Scrapers
{
    public class FtDotComSummaryTests
    {
        [TestFixture, Category("Integration")]
        public class When_fetching_data : AppHarborIgnoreFixture
        {
            static ScraperResults results;

            static readonly TickerSymbol[] Symbols = new[]
                                                         {
                                                             new TickerSymbol("AAPL", Exchange.Nasdaq), 
                                                             new TickerSymbol("GOOG", Exchange.Nasdaq), 
                                                             new TickerSymbol("ENRC", Exchange.Lse),
                                                             new TickerSymbol("GXY", Exchange.Asx)
                                                             //new TickerSymbol("TME", Exchange.Nzx)
                                                         };

            [Test, TestCaseSource("Symbols")]
            public void It_should_scrape_fundamentals(TickerSymbol tickerSymbol)
            {
                results = new FtDotComSummary().GetFundamentals(tickerSymbol);
                results.Url.Should().Not.Be.Null();
                AssertHelper.AssertFundamental<float>(results, "Open", Is.Not.EqualTo(0));
            }
        }

        [TestFixture, Category("Integration")]
        public class When_fetching_a_non_existent_symbol : AppHarborIgnoreFixture
        {
            [Test]
            public void It_should_throw_a_no_fundamentals_available_exception()
            {
                var symbol = new TickerSymbol("asdfb", Exchange.Nyse);
                var scraper = new FtDotComSummary();

                Assert.Throws<NoFundamentalsAvailableException>(() => scraper.GetFundamentals(symbol));
            }
        }
    }
}