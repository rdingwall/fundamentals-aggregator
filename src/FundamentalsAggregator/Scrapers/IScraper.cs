namespace FundamentalsAggregator.Scrapers
{
    public interface IScraper
    {
        ScraperResults GetFundamentals(TickerSymbol symbol);
    }
}