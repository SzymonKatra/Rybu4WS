using System;
using System.Collections.Generic;
using System.Text;

namespace Rybu4WS.Language
{
    public class ServerDeclaration
    {
        public string TypeName { get; set; }

        public List<ServerDependency> Dependencies { get; set; } = new List<ServerDependency>();

        public List<ServerVariable> Variables { get; set; } = new List<ServerVariable>();

        public List<ServerAction> Actions { get; set; } = new List<ServerAction>();
    }
}
