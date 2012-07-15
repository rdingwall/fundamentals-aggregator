namespace FundamentalsAggregator.TickerSymbolFormatters
{
    public interface ITickerSymbolFormatter
    {
        bool IsSupportedExchange(TickerSymbol symbol);
        string Format(TickerSymbol symbol);
    }
}