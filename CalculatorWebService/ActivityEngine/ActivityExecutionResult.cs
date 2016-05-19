using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorWebService.ActivityEngine
{
    using Newtonsoft.Json.Linq;

    public class ActivityExecutionResult
    {
        public string ErrorMessage { get; set; }
        public string Id { get; set; }
        public JToken Output { get; set; }
        public ExecutionStatus Status { get; set; }
    }

    public enum ExecutionStatus
    {
        Pending = 0,
        Running = 1,
        Completed = 2,
        Failed = 3,
    }
}
