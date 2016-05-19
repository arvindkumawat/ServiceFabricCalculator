namespace CalculatorWebService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Fabric;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;

    using global::CalculatorWebService.ActivityEngine;

    using Microsoft.ServiceFabric.Services.Client;
    using Microsoft.ServiceFabric.Services.Communication.Client;

    public class DefaultController : ApiController
    {
        [Route("activities"), HttpPost]
        public async Task<HttpResponseMessage> PostActivityRun()
        {
            ActivityExecutionResult result = new ActivityExecutionResult();
            result.Status = ExecutionStatus.Running;

            await Task.Delay(0).ConfigureAwait(false);
            return this.Request.CreateResponse(HttpStatusCode.Accepted);
        }

        [Route("activities/{id}"), HttpGet]
        public async Task<HttpResponseMessage> GetActivityStatus(string id)
        {
            ActivityExecutionResult result = new ActivityExecutionResult()
                {
                    Id = id,
                    Status = ExecutionStatus.Completed
                };

            await Task.Delay(0).ConfigureAwait(false);
            return this.Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [Route("activities/{id}"), HttpDelete]
        public async Task<HttpResponseMessage> DeleteActivity(string id)
        {
            await Task.Delay(0).ConfigureAwait(false);
            return this.Request.CreateResponse(HttpStatusCode.Accepted);
        }

        [Route("add/{a}/{b}")]
        public string GetAdd(int a, int b)
        {
            return (a + b).ToString();
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

        // POST api/values
        [Route("values1")]
        public void Post([FromBody]IList<string> values)
        {
            foreach (var value in values)
            {
                Trace.TraceInformation(value);
            }
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

        // DELETE api/values/5
        [Route("values1/{id}")]
        public void Delete(int id, [FromBody] string body)
        {
        }
    }
}
