using System;
using System.Runtime.Serialization;

namespace FundamentalsAggregator.TickerSymbolFormatters
{
    [Serializable]
    public class UnsupportedExchangeException : Exception
    {
        public UnsupportedExchangeException()
        {
        }

        public UnsupportedExchangeException(string message) : base(message)
        {
        }

        public UnsupportedExchangeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnsupportedExchangeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}