namespace FundamentalsAggregator.Scrapers
{
    public class MorningstarForwardValuation : MorningstarValuationBase
    {
        public MorningstarForwardValuation()
            : base(
                ajaxUrlFormat: "http://financials.morningstar.com/valuation/forward-valuation-list.action?t={0}&productCode=COM",
                viewUrlFormat: "http://financials.morningstar.com/ratios/price-ratio.html?t={0}",
                parser: new MorningStarForwardValuationTableParser())
        {
        }

        public override string ProviderName
        {
            get { return "Morningstar (Forward Valuation)"; }
        }
    }
}