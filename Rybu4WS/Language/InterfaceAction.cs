using System;
using System.Collections.Generic;
using System.Text;

namespace Rybu4WS.Language
{
    public class InterfaceAction
    {
        public string Name { get; set; }

        public List<string> RequiredReturnValues { get; set; } = new List<string>();
    }
}
