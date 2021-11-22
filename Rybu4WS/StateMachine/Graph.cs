using Rybu4WS.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.StateMachine
{
    public class Graph
    {
        public string Name { get; set; }

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

        public Edge CreateEdge(Node source, Node target, string receiveMessage) => CreateEdge(source, target, receiveMessage, (null, null));

        public Edge CreateEdge(Node source, Node target, string receiveMessage, (string serverName, string message) sendMessage)
        {
            var edge = new Edge()
            {
                Source = source,
                Target = target,
                ReceiveMessage = receiveMessage,
                SendMessageServer = sendMessage.serverName,
                SendMessage = sendMessage.message
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

        public string ToDedan(Language.System system)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"server: {Name}(agents A[N]:A; servers {string.Join(", ", system.GetAllDedanServerListExcept(Name))}),");
            
            sb.AppendLine("services {");
            var inputMessages = Edges.Select(x => x.ReceiveMessage)
                .Concat(Edges.Where(x => x.SendMessageServer == Name).Select(x => x.SendMessage))
                .Distinct().ToList();
            if (inputMessages.Count > 0)
            {
                for (int i = 0; i < inputMessages.Count; i++)
                {
                    sb.Append($"    {inputMessages[i]}");
                    if (i != inputMessages.Count - 1) sb.Append(',');
                    sb.AppendLine();
                }
            }
            else
            {
                sb.AppendLine("    PLACEHOLDER_DO_NOTHING");
            }
            sb.AppendLine("},");

            sb.AppendLine("states {");
            var states = Nodes.Select(x => x.ToString()).ToList();
            for (int i = 0; i < states.Count; i++)
            {
                sb.Append($"    {states[i]}");
                if (i != states.Count - 1) sb.Append(',');
                sb.AppendLine();
            }
            sb.AppendLine("},");

            sb.AppendLine("actions {");
            for (int i = 0; i < Edges.Count; i++)
            {
                var edge = Edges[i];
                var actionResult = edge.IsSendingMessage() ? $"A[j].{edge.SendMessageServer}.{edge.SendMessage}, {Name}.{edge.Target}" : $"{Name}.{edge.Target}";
                sb.Append($"    <j=1..N>{{A[j].{Name}.{edge.ReceiveMessage}, {Name}.{edge.Source}}} -> {{{actionResult}}}");
                if (i != Edges.Count - 1) sb.Append(',');
                sb.AppendLine();
            }
            sb.AppendLine("};");

            return sb.ToString();
        }
    }
}
