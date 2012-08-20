using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace FundamentalsAggregator
{
    public class ProviderResults
    {
        public ProviderResults(string providerName, Uri url, IDictionary<string, string> fundamentals)
        {
            if (providerName == null) throw new ArgumentNullException("providerName");
            if (url == null) throw new ArgumentNullException("url");
            if (fundamentals == null) throw new ArgumentNullException("fundamentals");
            ProviderName = providerName;
            Url = url;
            Fundamentals = fundamentals;
        }

        public ProviderResults(string providerName, Exception error)
        {
            if (providerName == null) throw new ArgumentNullException("providerName");
            ProviderName = providerName;
            Error = error;
        }

        public ProviderResults(string providerName)
        {
            if (providerName == null) throw new ArgumentNullException("providerName");
            ProviderName = providerName;
            NoFundamentalsAvailable = true;
        }

        [DefaultValue(false)]
        public bool NoFundamentalsAvailable { get; private set; }

        public string ProviderName { get; private set; }
        public Uri Url { get; private set; }
        public IDictionary<string, string> Fundamentals { get; private set; }
        public Exception Error { get; private set; }

        [DefaultValue(false)]
        public bool IsError { get { return Error != null; } }
    }
}