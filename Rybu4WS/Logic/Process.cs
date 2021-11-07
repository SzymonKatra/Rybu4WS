using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.Logic
{
    public class Process
    {
        public string Name { get; set; }

        public List<BaseStatement> Statements { get; set; } = new List<BaseStatement>();
    }
}
