using System;
using System.ComponentModel;

namespace FundamentalsAggregator
{
    public class FundamentalResult
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public string ToolTip { get; set; }

        public bool HasToolTip { get { return !String.IsNullOrEmpty(ToolTip); } }

        [DefaultValue(false)]
        public bool IsHighlighted { get; set; }

        public bool NameEquals(string name)
        {
            if (name == null) throw new ArgumentNullException("name");

            return String.Equals((Name ?? "").Trim(),
                                 name, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}