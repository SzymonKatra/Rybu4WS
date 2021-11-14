using Rybu4WS.Logic;
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

        public List<Node> Nodes { get; set; } = new List<Node>();

        public List<Edge> Edges { get; set; } = new List<Edge>();

        public Node GetOrCreateIdleNode(List<StatePair> states)
        {
            var node = Nodes.FirstOrDefault(x => x.Caller == null && x.CodeLocation == null && CompareStates(x.States, states));

            if (node == null)
            {
                node = new Node() { States = new List<StatePair>(states) };
                Nodes.Add(node);
            }

            return node;
        }

        public Edge CreateEdge(Node source, Node target, string receiveMessage, string sendMessage = null)
        {
            var edge = new Edge()
            {
                Source = source,
                Target = target,
                ReceiveMessage = receiveMessage,
                SendMessage = sendMessage
            };

            source.OutEdges.Add(edge);
            this.Edges.Add(edge);

            return edge;
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
