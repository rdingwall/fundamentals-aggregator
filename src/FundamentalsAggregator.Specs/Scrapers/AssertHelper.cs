using System;
using FundamentalsAggregator.Scrapers;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace FundamentalsAggregator.Specs.Scrapers
{
    public static class AssertHelper
    {
        public static void AssertFundamental<T>(ScraperResults results, string key, Constraint constraint)
        {
            if (!results.Fundamentals.ContainsKey(key))
                Assert.Fail("Missing fundamental: {0}. Found: {1}", key,
                    String.Join(", ", results.Fundamentals.Keys));

            var orig = results.Fundamentals[key];

            var value = default(T);

            try
            {
                value = (T)Convert.ChangeType(orig, typeof(T));

            }
            catch (FormatException)
            {
                Assert.Fail("Could not parse fundamental '{0}' = '{1}' as a {2}.", key, orig, typeof(T));
            }


            Assert.That(value, constraint);
        }
    }
}