using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.Dedan
{
    public class Action
    {
        public string ServerTypeName { get; set; }

        public string ServiceName { get; set; }

        public string PreState { get; set; }

        public string PostState { get; set; }

        public string SendMessage { get; set; }

        public string OutMessageServerTypeName { get; set; }

        public string OutMessageServiceName { get; set; }

        public string ToDedan(int agentCount)
        {
            return $"<j=1..{agentCount}>{{A[j].{ServerTypeName}.{ServiceName}, {ServerTypeName}.{PreState}}} -> {{A[j].{OutMessageServerTypeName}.{OutMessageServiceName}, {ServerTypeName}.{PostState}}}";
        }
    }
}
