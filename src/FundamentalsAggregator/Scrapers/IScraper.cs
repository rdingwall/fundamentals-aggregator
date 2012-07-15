namespace FundamentalsAggregator.Scrapers
{
    public interface IScraper
    {
        string ProviderName { get; }
        ScraperResults GetFundamentals(TickerSymbol symbol);
    }
}