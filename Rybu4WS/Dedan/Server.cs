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

        public List<(string name, string serverType)> ServerDependencies { get; set; }

        public List<string> Services { get; set; }

        public List<string> States { get; set; }


    }
}
