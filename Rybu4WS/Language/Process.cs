using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.Language
{
    public class Process
    {
        public string Name { get; set; }

        public string ServerName => $"Process{Name}";

        public int AgentIndex { get; set; }

        public List<BaseStatement> Statements { get; set; } = new List<BaseStatement>();
    }
}
