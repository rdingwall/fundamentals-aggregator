﻿using System;

namespace FundamentalsAggregator.TickerSymbolFormatters
{
    public class MorningStarTickerSymbolFormatter : ITickerSymbolFormatter
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
                case Exchange.Nyse:
                    return name;

                case Exchange.Lse:
                    return "XLON:" + name;

                case Exchange.Asx:
                    return "XASX:" + name;

                default:
                    throw new UnsupportedExchangeException(
                        String.Format("Don't know how to get data for {0} exchange from MorningStar.", symbol.Exchange));
            }
        }
    }
}