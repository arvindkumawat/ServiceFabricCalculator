namespace CalculatorHistoryService.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Microsoft.ServiceFabric.Data;
    using Microsoft.ServiceFabric.Data.Collections;

    [RoutePrefix("api")]
    public class DefaultController : ApiController
    {
        private readonly IReliableStateManager stateManager;

        public DefaultController(IReliableStateManager stateManager)
        {
            this.stateManager = stateManager;
        }

        [HttpGet]
        [Route("get/count")]
        public async Task<IHttpActionResult> GetCount()
        {
            IReliableDictionary<string, long> historyDictionary = await this.stateManager.GetOrAddAsync<IReliableDictionary<string, long>>("historyDictionary");

            using (ITransaction tx = this.stateManager.CreateTransaction())
            {
                ConditionalResult<long> result = await historyDictionary.TryGetValueAsync(tx, "Number of calculations Performed");

                if (result.HasValue)
                {
                    return this.Ok(result.Value);
                }
            }

            return this.Ok(0);
        }

        [HttpGet]
        [Route("get/history")]
        public async Task<IEnumerable<string>> GetHistory()
        {
            IList<string> history = new List<string>();
            IReliableQueue<string> calcHistoryQueue = await this.stateManager.GetOrAddAsync<IReliableQueue<string>>("CalcHistoryQueue");
            IEnumerator<string> enumerator = calcHistoryQueue.GetEnumerator();
            while (enumerator.MoveNext())
            {
                history.Add(enumerator.Current);
            }

            return history;
        }


        [HttpPut]
        [Route("add/{calc}")]
        public async Task<IHttpActionResult> Add(string calc)
        {
            IReliableQueue<string> calcHistoryQueue = await this.stateManager.GetOrAddAsync<IReliableQueue<string>>("CalcHistoryQueue");
            IReliableDictionary<string, long> historyDictionary = await this.stateManager.GetOrAddAsync<IReliableDictionary<string, long>>("historyDictionary");

            using (ITransaction tx = this.stateManager.CreateTransaction())
            {
                await calcHistoryQueue.EnqueueAsync(tx, calc);
                await historyDictionary.AddOrUpdateAsync(tx, "Number of calculations Performed", 0, (k, v) => ++v);

                await tx.CommitAsync();
            }

            return this.Ok();
        }
    }
}
