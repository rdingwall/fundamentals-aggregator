using System;
using System.Web.Management;

namespace FundamentalsAggregator.Mvc
{
    public class LogEvent : WebRequestErrorEvent
    {
        public LogEvent(string message)
            : base(null, null, 100001, new Exception(message))
        {
        }

        public LogEvent(Exception e)
            : base(null, null, 100001, e)
        {
        }
    }
}