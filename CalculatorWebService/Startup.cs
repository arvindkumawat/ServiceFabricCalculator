namespace CalculatorWebService
{
    using Owin;
    using System.Web.Http;
    using System.Web.Http.Batch;

    using App_Start;

    public static class Startup
    {
        public static void ConfigureApp(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            HttpServer server = new HttpServer(config);

            config.Routes.MapHttpBatchRoute(
                routeName: "batch",
                routeTemplate: "batch",
                batchHandler: new DefaultHttpBatchHandler(server));

            config.MapHttpAttributeRoutes();
            FormatterConfig.ConfigureFormatters(config.Formatters);
            
            appBuilder.UseWebApi(config);
        }
    }
}
