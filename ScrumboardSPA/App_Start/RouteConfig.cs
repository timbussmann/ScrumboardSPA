namespace ScrumboardSPA
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // by default it's not possible to configure routing for an uri containing a dot.
            // asp will search for a file with this ending in the specified directory instead of 
            // calling the configured route. It is possible to reconfigure this behaviour.
            routes.MapRoute(
                name: "cache.manifest",
                url: "manifest-appcache",
                defaults: new {controller = "Home", action = "GetCacheManifest"});

            routes.MapRoute(
                name: "Views",
                url: "views/{viewName}",
                defaults: new {controller = "Home", action = "GetView"});

            routes.MapRoute(
                name: "Default",
                url: "{*url}",
                defaults: new { controller = "Home", action = "Index"});
        }
    }
}