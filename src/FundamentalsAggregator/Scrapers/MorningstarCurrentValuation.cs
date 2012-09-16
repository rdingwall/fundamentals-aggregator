namespace FundamentalsAggregator.Scrapers
{
    public class MorningstarCurrentValuation : MorningstarValuationBase
    {
        public MorningstarCurrentValuation()
            : base(
                ajaxUrlFormat: "http://financials.morningstar.com/valuation/current-valuation-list.action?t={0}&productCode=COM",
                parser: new MorningStarCurrentValuationTableParser())
        {
        }

        public override string ProviderName
        {
            get { return "Morningstar (Current Valuation)"; }
        }
    }
}