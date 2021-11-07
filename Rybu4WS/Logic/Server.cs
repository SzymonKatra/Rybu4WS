using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.Logic
{
    public class Server
    {
        public string Name { get; set; }

        public List<ServerDependency> Dependencies { get; set; } = new List<ServerDependency>();

        public List<ServerVariable> Variables { get; set; } = new List<ServerVariable>();

        public List<ServerAction> Actions { get; set; } = new List<ServerAction>();

        public List<string> GetStates()
        {
            var states = new List<string>() { "" };

            foreach (var variable in Variables)
            {
                var newStates = new List<string>();
                foreach (var state in states)
                {
                    foreach (var value in variable.AvailableValues)
                    {
                        newStates.Add($"{state}{(state != "" ? "_" : "")}{variable.Name}_{value}");
                    }
                }
                states = newStates;
            }

            return states;
        }
    }
}
