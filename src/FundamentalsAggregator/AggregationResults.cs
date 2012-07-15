using System;
using System.Collections.Generic;

namespace FundamentalsAggregator
{
    public class AggregationResults
    {
        public AggregationResults(
            TickerSymbol symbol, 
            IEnumerable<ProviderResults> providers, 
            DateTime timestamp)
        {
            if (symbol == null) throw new ArgumentNullException("symbol");
            Timestamp = timestamp;
            Symbol = symbol;
            Providers = providers;
        }

        public DateTime Timestamp { get; private set; }
        public TickerSymbol Symbol { get; private set; }

        public IEnumerable<ProviderResults> Providers { get; private set; }
    }
}