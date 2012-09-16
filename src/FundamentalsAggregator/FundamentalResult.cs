using System.ComponentModel;

namespace FundamentalsAggregator
{
    public class FundamentalResult
    {
        public string Name { get; set; }
        public string Value { get; set; }

        [DefaultValue(false)]
        public bool IsHighlighted { get; set; }
    }
}