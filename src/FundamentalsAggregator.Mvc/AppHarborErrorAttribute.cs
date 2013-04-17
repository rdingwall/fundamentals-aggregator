using System.Web.Mvc;

namespace FundamentalsAggregator.Mvc
{
    public class AppHarborErrorAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            // From http://blog.appharbor.com/2011/12/20/super-simple-logging-on-appharbor
            new LogEvent(filterContext.Exception).Raise();
        }
    }
}