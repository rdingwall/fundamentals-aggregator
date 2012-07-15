using System;
using System.Runtime.Serialization;

namespace FundamentalsAggregator.Scrapers
{
    [Serializable]
    public class NoFundamentalsAvailableException : Exception
    {
        public NoFundamentalsAvailableException()
        {
        }

        public NoFundamentalsAvailableException(string message) : base(message)
        {
        }

        public NoFundamentalsAvailableException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoFundamentalsAvailableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}