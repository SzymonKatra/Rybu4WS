using Rybu4WS.StateMachine;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.Language
{
    public class Server
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public List<ServerVariable> Variables { get; set; } = new List<ServerVariable>();

        public List<ServerAction> Actions { get; set; } = new List<ServerAction>();

        public List<string> ImplementedInterfaces { get; set; } = new List<string>();
    }
}
