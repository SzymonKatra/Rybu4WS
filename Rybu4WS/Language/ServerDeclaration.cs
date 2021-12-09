using System;
using System.Collections.Generic;
using System.Text;

namespace Rybu4WS.Language
{
    public class ServerDeclaration : IWithCodeLocation
    {
        public string TypeName { get; set; }

        public List<ServerDependency> Dependencies { get; set; } = new List<ServerDependency>();

        public List<ServerImplementedInterface> ImplementedInterfaces { get; set; } = new List<ServerImplementedInterface>();

        public List<VariableDeclaration> Variables { get; set; } = new List<VariableDeclaration>();

        public List<ServerAction> Actions { get; set; } = new List<ServerAction>();

        public CodeLocation CodeLocation { get; set; }
    }
}
