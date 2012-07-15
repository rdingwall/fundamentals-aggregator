using System;

namespace FundamentalsAggregator.TickerSymbolFormatters
{
    public class FtDotComFormatter : ITickerSymbolFormatter
    {
        public bool IsSupportedExchange(TickerSymbol symbol)
        {
            switch (symbol.Exchange)
            {
                case Exchange.Lse:
                case Exchange.Nasdaq:
                case Exchange.Asx:
                case Exchange.Nyse:
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
                    return name + ":NSQ";

                case Exchange.Nyse:
                    return name + ":NYQ";

                case Exchange.Lse:
                    return name + ":LSE";

                case Exchange.Asx:
                    return name + ":ASX";

                default:
                    throw new UnsupportedExchangeException(
                        String.Format("Don't know how to get data for {0} exchange from ft.com.", symbol.Exchange));
            }
        }
    }
}