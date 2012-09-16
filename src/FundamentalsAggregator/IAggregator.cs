namespace FundamentalsAggregator
{
    public interface IAggregator
    {
        AggregationResults Aggregate(TickerSymbol symbol);
    }
}