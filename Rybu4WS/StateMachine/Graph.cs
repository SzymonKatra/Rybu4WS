using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.StateMachine
{
    public class Graph
    {
        public Node InitNode { get; set; }

        public List<Node> Nodes { get; set; }

        public List<Edge> Edges { get; set; }

        public Node GetOrCreateIdleNode(List<StatePair> states)
        {
            var node = Nodes.FirstOrDefault(x => x.Caller == null && x.CodeLocation == null && CompareStates(x.States, states));

            if (node == null)
            {
                node = new Node() { States = new List<StatePair>(states) };
            }

            return node;
        }

        private bool CompareStates(List<StatePair> a, List<StatePair> b)
        {
            if (a.Count != b.Count) return false;

            for (int i = 0; i < a.Count; i++)
            {
                if (a[i].Name != b[i].Name || a[i].Value != b[i].Value) return false;
            }

            return true;
        }
    }
}
