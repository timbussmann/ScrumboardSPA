namespace ScrumboardSPA
{
    using System.Web.Http;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}/{action}",
                defaults: new {id = RouteParameter.Optional, action = RouteParameter.Optional}
                );

            // Allows updating the story state via PUT /story/{id}/state/done:
            config.Routes.MapHttpRoute(
                name: "Story State",
                routeTemplate: "api/story/{id}/state/{state}",
                defaults: new {controller = "story", action = "state"});
        }
    }
}
