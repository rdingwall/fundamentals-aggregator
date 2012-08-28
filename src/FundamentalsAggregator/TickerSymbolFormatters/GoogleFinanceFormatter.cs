using System;

namespace FundamentalsAggregator.TickerSymbolFormatters
{
    public class GoogleFinanceFormatter : ITickerSymbolFormatter
    {
        public bool IsSupportedExchange(TickerSymbol symbol)
        {
            switch (symbol.Exchange)
            {
                // Full list is here: http://www.google.com/googlefinance/disclaimer/
                case Exchange.Lse:
                case Exchange.Nasdaq:
                case Exchange.Asx:
                case Exchange.Nyse:
                case Exchange.Nzx:
                    return true;
            }

            return false;
        }

        public string Format(TickerSymbol symbol)
        {
            if (symbol == null) throw new ArgumentNullException("symbol");

            var name = symbol.Symbol.ToUpper();

            switch (symbol.Exchange)
            {
                case Exchange.Nasdaq:
                    return "NASDAQ:" + name; 

                case Exchange.Nyse:
                    return "NYSE:" + name;

                case Exchange.Lse:
                    return "LON:" + name;

                case Exchange.Asx:
                    return "ASX:" + name;

                case Exchange.Nzx:
                    return "NZX:" + name;

                default:
                    throw new UnsupportedExchangeException(
                        String.Format("Don't know how to get data for {0} exchange from Google Finance.", symbol.Exchange));
            }
        }
    }
}