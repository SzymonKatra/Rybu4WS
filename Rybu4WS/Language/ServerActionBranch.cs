using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.Language
{
    public class ServerActionBranch
    {
        public ICondition Condition { get; set; }

        public List<BaseStatement> Statements { get; set; } = new List<BaseStatement>();

        public TimedDelay ExecutionDelay { get; set; }
    }
}
