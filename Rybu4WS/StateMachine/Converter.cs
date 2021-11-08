using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.StateMachine
{
    public class Converter
    {
        public Graph Convert(Logic.Server server)
        {
            var graph = new Graph();

            var initState = new List<StatePair>();
            foreach (var item in server.Variables)
            {
                initState.Add(new StatePair() { Name = item.Name, Value = item.InitialValue });
            }

            graph.InitNode = graph.GetOrCreateIdleNode(initState);

            foreach (var action in server.Actions)
            {
                foreach (var branch in action.Branches)
                {
                    var preStates = server.GetCartesianStates(branch.Condition);
                    var firstStatement = branch.Statements.First();
                }
            }

            return graph;
        }
    }
}
