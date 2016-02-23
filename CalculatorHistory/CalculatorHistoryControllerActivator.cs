namespace CalculatorHistory
{
    using System;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;

    using CalculatorHistoryService.Controllers;

    using Microsoft.ServiceFabric.Data;

    class CalculatorHistoryControllerActivator : IHttpControllerActivator
    {
        private readonly IReliableStateManager stateManager;
        
        public CalculatorHistoryControllerActivator(IReliableStateManager stateManager)
        {
            if (stateManager == null)
            {
                throw new ArgumentNullException(nameof(stateManager));
            }

            this.stateManager = stateManager;
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            IHttpController controller = controller = new DefaultController(this.stateManager);
            var disposable = controller as IDisposable;
            request.RegisterForDispose(disposable);

            return controller;
        }
    }
}
