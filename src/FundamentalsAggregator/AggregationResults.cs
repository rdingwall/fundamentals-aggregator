using System;
using System.Collections.Generic;

namespace FundamentalsAggregator
{
    public class AggregationResults
    {
        public AggregationResults(
            TickerSymbol symbol, 
            IEnumerable<ProviderResults> providers, 
            DateTime timestamp,
            string longName)
        {
            if (symbol == null) throw new ArgumentNullException("symbol");
            Timestamp = timestamp;
            LongName = longName ?? "";
            Symbol = symbol;
            Providers = providers;
        }

        public DateTime Timestamp { get; private set; }
        public string LongName { get; private set; }
        public TickerSymbol Symbol { get; private set; }
        public bool HasLongName { get { return !String.IsNullOrWhiteSpace(LongName); } }

        public IEnumerable<ProviderResults> Providers { get; private set; }
    }
}