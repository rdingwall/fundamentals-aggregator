namespace FundamentalsAggregator.Scrapers
{
    public class MorningstarCurrentValuation : MorningstarValuationBase
    {
        public MorningstarCurrentValuation()
            : base(
                ajaxUrlFormat: "http://financials.morningstar.com/valuation/current-valuation-list.action?t={0}&productCode=COM",
                viewUrlFormat: "http://financials.morningstar.com/ratios/price-ratio.html?t={0}",
                parser: new MorningStarCurrentValuationTableParser())
        {
        }

        public override string ProviderName
        {
            get { return "Morningstar (Current Valuation)"; }
        }
    }
}