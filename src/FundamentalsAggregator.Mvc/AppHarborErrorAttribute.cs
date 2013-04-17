using System.Web.Mvc;

namespace FundamentalsAggregator.Mvc
{
    public class AppHarborErrorAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            new LogEvent(filterContext.Exception).Raise();
        }
    }
}