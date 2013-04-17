using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace FundamentalsAggregator
{
    public class ProviderResults
    {
        public ProviderResults()
        {
            Fundamentals = Enumerable.Empty<FundamentalResult>();
        }

        public ProviderResults(string providerName, Uri url, IEnumerable<FundamentalResult> fundamentals) : this()
        {
            if (providerName == null) throw new ArgumentNullException("providerName");
            if (url == null) throw new ArgumentNullException("url");
            if (fundamentals == null) throw new ArgumentNullException("fundamentals");
            ProviderName = providerName;
            Url = url;
            Fundamentals = fundamentals;
        }

        public ProviderResults(string providerName, Exception error) : this()
        {
            if (providerName == null) throw new ArgumentNullException("providerName");
            ProviderName = providerName;
            Error = error;
        }

        public ProviderResults(string providerName) : this()
        {
            if (providerName == null) throw new ArgumentNullException("providerName");
            ProviderName = providerName;
            NoFundamentalsAvailable = true;
        }

        [DefaultValue(false)]
        public bool NoFundamentalsAvailable { get; private set; }

        public string ProviderName { get; private set; }
        public Uri Url { get; private set; }
        public IEnumerable<FundamentalResult> Fundamentals { get; private set; }
        public Exception Error { get; private set; }

        [DefaultValue(false)]
        public bool IsError { get { return Error != null; } }
    }
}