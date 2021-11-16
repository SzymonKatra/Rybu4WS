using Rybu4WS.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.TrailDebugger
{
    public class DebuggerStep
    {
        public Dictionary<string, string> Variables { get; set; }

        public CodeLocation? CodeLocation { get; set; }

        public bool? IsPreCodeLocation { get; set; }
    }
}
