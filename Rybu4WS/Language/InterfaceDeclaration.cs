using System;
using System.Collections.Generic;
using System.Text;

namespace Rybu4WS.Language
{
    public class InterfaceDeclaration
    {
        public string TypeName { get; set; }

        public List<InterfaceAction> RequiredActions { get; set; } = new List<InterfaceAction>();
    }
}
