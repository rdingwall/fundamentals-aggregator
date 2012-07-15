using log4net.Config;
using NUnit.Framework;

namespace FundamentalsAggregator.Specs
{
    [SetUpFixture]
    public class BeforeAllTests
    {
        [SetUp]
        public void SetUp()
        {
            BasicConfigurator.Configure();
        }
    }
}
