using System;
using System.Configuration;
using NUnit.Framework;

namespace FundamentalsAggregator.Specs.Scrapers
{
    public class AppHarborIgnoreFixture
    {
        [SetUp]
        public virtual void SetUp()
        {
            bool isRunningInAppHarbor = String.Equals(
                ConfigurationManager.AppSettings["Environment"], "test",
                StringComparison.CurrentCultureIgnoreCase);

            if (isRunningInAppHarbor)
                Assert.Ignore("Ignored in AppHarbor");
        }
    }
}