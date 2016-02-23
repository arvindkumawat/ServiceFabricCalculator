namespace CalculatorHistoryService
{
    using System.Web.Http;
    using System.Web.Http.Dispatcher;

    using Microsoft.ServiceFabric.Data;
    using Owin;
    using App_Start;

    using CalculatorHistory;

    /// <summary>
    /// OWIN configuration
    /// </summary>
    public class Startup : IOwinAppBuilder
    {
        private readonly IReliableStateManager stateManager;

        public Startup(IReliableStateManager stateManager)
        {
            this.stateManager = stateManager;
        }

        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            FormatterConfig.ConfigureFormatters(config.Formatters);
            config.Services.Replace(typeof(IHttpControllerActivator), new CalculatorHistoryControllerActivator(this.stateManager));


            appBuilder.UseWebApi(config);
        }
    }
}
