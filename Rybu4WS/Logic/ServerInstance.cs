using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.Logic
{
    public class ServerInstance
    {
        public string Name { get; set; }

        public string ServerType { get; set; }

        public Dictionary<string, string> InitialStates { get; set; } = new Dictionary<string, string>();

        public List<string> DependencyInstancesNames { get; set; } = new List<string>();
    }
}
