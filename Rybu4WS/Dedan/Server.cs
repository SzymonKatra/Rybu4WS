using Rybu4WS.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.Dedan
{
    public class Server
    {
        public string TypeName { get; set; }

        public List<(string name, string serverType)> ServerDependencies { get; set; } = new List<(string name, string serverType)>();

        public List<string> Services { get; set; } = new List<string>();

        public HashSet<string> States { get; set; } = new HashSet<string>();

        public List<Dedan.Action> Actions { get; set; }
    }
}
