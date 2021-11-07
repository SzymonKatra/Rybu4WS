using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.Dedan
{
    public class Action
    {
        public string SourceState { get; set; }

        public string TargetState { get; set; }

        public string TargetServerName { get; set; }

        public string TargetServiceName { get; set; }

        public string ToDedan(int agentCount)
        {
            throw new NotImplementedException();
        }
    }
}
