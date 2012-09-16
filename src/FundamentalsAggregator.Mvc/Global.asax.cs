using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace FundamentalsAggregator.Mvc
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        IWindsorContainer container;

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Fundamentals", // Route name
                "fundamentals/{exchange}/{symbol}", // URL with parameters
                new { controller = "Home", action = "Fundamentals" } // Parameter defaults
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            container = new WindsorContainer();
            container.Register(
                Component.For<IAggregator>().Instance(new ReadThroughResultCache(new Aggregator())),
                AllTypes.FromThisAssembly().BasedOn<Controller>().LifestyleTransient());
            DependencyResolver.SetResolver(new MvcWindsorServiceLocator(container));

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container.Kernel));

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        public override void Dispose()
        {
            container.Dispose();
            base.Dispose();
        }
    }
}