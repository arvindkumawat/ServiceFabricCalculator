namespace CalculatorWebService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Fabric;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Microsoft.ServiceFabric.Services.Client;
    using Microsoft.ServiceFabric.Services.Communication.Client;

    [RoutePrefix("api")]
    public class DefaultController : ApiController
    {
        private const string CalculatorHistoryServiceName = "fabric:/SFCalculator/CalculatorHistoryService";
        private const int MaxQueryRetryCount = 20;
        private static TimeSpan BackoffQueryDelay = TimeSpan.FromSeconds(3);
        private static FabricClient fabricClient = new FabricClient();

        private static CommunicationClientFactory clientFactory = new CommunicationClientFactory(
            ServicePartitionResolver.GetDefault(),
            TimeSpan.FromSeconds(10),
            TimeSpan.FromSeconds(3));

        [Route("add/{a}/{b}")]
        public async Task<HttpResponseMessage> GetAdd(int a, int b)
        {
            int result = a + b;
            string str = $"{a} + {b} = {result}";

            // Determine the partition key that should handle the request
            long partitionKey = 1;

            // Use service partition client to resolve the service and partition key.
            // This determines the endpoint of the replica that should handle the request.
            // Internally, the service partition client handles exceptions and retries appropriately.
            ServicePartitionClient<CommunicationClient> servicePartitionClient = new ServicePartitionClient<CommunicationClient>(
                clientFactory,
                new Uri(CalculatorHistoryServiceName),
                partitionKey);

            return await servicePartitionClient.InvokeWithRetryAsync(
                client =>
                {
                    Uri serviceAddress = new Uri(client.BaseAddress, string.Format("api/add/{0}", str));

                    HttpWebRequest request = WebRequest.CreateHttp(serviceAddress);
                    request.Method = "PUT";
                    request.ContentLength = 0;
                    request.Timeout = (int)client.OperationTimeout.TotalMilliseconds;
                    request.ReadWriteTimeout = (int)client.ReadWriteTimeout.TotalMilliseconds;

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        HttpResponseMessage message = new HttpResponseMessage();
                        message.Content = new StringContent(
                            String.Format("<h1>{0}</h1> added to partition <h2>{1}</h2> at {2}", str, client.ResolvedServicePartition.Info.Id, serviceAddress),
                            Encoding.UTF8,
                            "text/html");

                        return Task.FromResult<HttpResponseMessage>(message);
                    }
                });
        }

        [Route("sub/{a}/{b}")]
        public string GetSub(int a, int b)
        {
            return (a - b).ToString();
        }

        [Route("mul/{a}/{b}")]
        public string GetMul(int a, int b)
        {
            return (a * b).ToString();
        }
        [Route("div/{a}/{b}")]
        public string GetDiv(int a, int b)
        {
            return (a / b).ToString();
        }

        // GET api/values
        [Route("values")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [Route("values/{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [Route("values")]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [Route("values/{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [Route("values/{id}")]
        public void Delete(int id)
        {
        }
    }
}
