namespace FundamentalsAggregator.Scrapers
{
    public class MorningstarForwardValuation : MorningstarValuationBase
    {
        public MorningstarForwardValuation()
            : base(
                ajaxUrlFormat: "http://financials.morningstar.com/valuation/forward-valuation-list.action?t={0}&productCode=COM",
                parser: new MorningStarForwardValuationTableParser())
        {
        }

        public override string ProviderName
        {
            get { return "Morningstar (Forward Valuation)"; }
        }
    }
}