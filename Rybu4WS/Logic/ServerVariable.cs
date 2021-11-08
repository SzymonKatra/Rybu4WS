using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.Logic
{
    public class ServerVariable
    {
        public string Name { get; set; }

        public VariableType Type { get; set; }

        public List<string> AvailableValues { get; set; } = new List<string>();

        public string InitialValue { get; set; }
    }
}
