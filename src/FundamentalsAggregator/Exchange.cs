using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FundamentalsAggregator
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Exchange
    {
        Nyse,
        Asx,
        Lse,
        Nasdaq,
        Nzx
    }
}