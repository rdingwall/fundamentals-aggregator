using NUnit.Framework;
using SharpTestsEx;

namespace FundamentalsAggregator.Specs
{
    public class NumberParserTests
    {
        public class When_parsing_a_regular_double
        {
            [Test]
            public void It_should_parse_correctly()
            {
                double result;
                NumberParser.TryParse("0.1", out result);
                Assert.AreEqual(0.1, result, 0.001);
            }

            [Test]
            public void It_should_return_true()
            {
                double result;
                Assert.IsTrue(NumberParser.TryParse("0.1", out result));
            }
        }

        public class When_parsing_a_percentage
        {
            [Test]
            public void It_should_parse_correctly()
            {
                double result;
                NumberParser.TryParse("12.59%", out result);
                Assert.AreEqual(0.1259, result, 0.0001);
            }

            [Test]
            public void It_should_return_true()
            {
                double result;
                Assert.IsTrue(NumberParser.TryParse("12.59%", out result));
            }
        }
    }
}