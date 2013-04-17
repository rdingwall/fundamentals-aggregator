using System;

namespace FundamentalsAggregator
{
    /// <summary>
    /// Parses either:
    /// 
    /// 1. 12.59% => 0.1259
    /// 2. 0.1259 => 0.1259
    /// </summary>
    public static class NumberParser
    {
        public static bool TryParse(string value, out double result)
        {
            if (value == null) throw new ArgumentNullException("value");

            if (value.Contains("%"))
            {
                value = value.Replace("%", "");
                if (!Double.TryParse(value, out result))
                    return false;

                result /= 100;
                return true;
            }

            return Double.TryParse(value, out result);
        }
    }
}