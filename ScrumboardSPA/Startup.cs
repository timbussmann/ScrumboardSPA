namespace ScrumboardSPA
{
    using Microsoft.AspNet.SignalR;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new StringEnumConverter());
            GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);

            app.MapSignalR();
        }
    }
}