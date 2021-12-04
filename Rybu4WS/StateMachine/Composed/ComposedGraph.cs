using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rybu4WS.StateMachine.Composed
{
    public class ComposedGraph
    {
        private ListStatePairEqualityComparer _listStatePairComparer = new ListStatePairEqualityComparer();

        public string Name { get; set; }

        public ComposedNode InitNode { get; set; }

        public List<ComposedNode> Nodes { get; set; } = new List<ComposedNode>();

        public List<ComposedEdge> Edges { get; set; } = new List<ComposedEdge>();

        public int[] RequiredAgents { get; set; }

        public ComposedNode GetOrCreateNode(Dictionary<int, Node> baseNodes, List<StatePair> states, out bool isNew)
        {
            ComposedNode result = null;
            foreach (var item in Nodes)
            {
                if (!CompareStates(item.States, states)) continue;

                bool isMatch = true;
                foreach (var kvp in baseNodes)
                {
                    if (!item.Agents.ContainsKey(kvp.Key)) throw new Exception("Existing nodes does not contain the same amout of agents");
                    if (item.Agents[kvp.Key].BaseNodeReference != kvp.Value)
                    {
                        isMatch = false;
                        break;
                    }
                }
                if (isMatch)
                {
                    result = item;
                    break;
                }
            }

            isNew = result == null;
            if (isNew)
            {
                result = new ComposedNode() { States = new List<StatePair>(states) };
                foreach (var (agentIndex, baseNode) in baseNodes)
                {
                    var agentState = new ComposedNode.AgentState()
                    {
                        BaseNodeReference = baseNode,
                        Caller = baseNode.Caller,
                        CodeLocation = baseNode.CodeLocation,
                        IsPending = baseNode.IsPending
                    };
                    result.Agents.Add(agentIndex, agentState);
                }
                Nodes.Add(result);
            }

            return result;
        }

        public ComposedEdge GetOrCreateEdge(int agentIndex, ComposedNode source, ComposedNode target, string receiveMessage, (string serverName, string message) sendMessage, out bool isNew)
        {
            var edge = Edges.SingleOrDefault(x =>
                x.AgentIndex == agentIndex &&
                x.Source == source &&
                x.Target == target &&
                x.ReceiveMessage == receiveMessage &&
                x.SendMessage == sendMessage.message &&
                x.SendMessageServer == sendMessage.serverName);

            isNew = edge == null;
            if (isNew)
            {
                edge = new ComposedEdge()
                {
                    AgentIndex = agentIndex,
                    Source = source,
                    Target = target,
                    ReceiveMessage = receiveMessage,
                    SendMessage = sendMessage.message,
                    SendMessageServer = sendMessage.serverName
                };
                Edges.Add(edge);
                source.Agents[agentIndex].OutEdges.Add(edge);
                target.Agents[agentIndex].InEdges.Add(edge);
            }

            return edge;
        }

        private bool CompareStates(List<StatePair> a, List<StatePair> b)
        {
            return _listStatePairComparer.Equals(a, b);
        }

        public string ToDedan(Language.System system)
        {
            var sb = new StringBuilder();

            var serversParameters = system.GetAllDedanServerListExcept(Name).ToList();
            var serversStr = serversParameters.Count > 0 ? $"; servers {string.Join(", ", serversParameters)}" : "";
            sb.AppendLine($"server: {Name}(agents A[N]:A{serversStr}),");

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
                var actionResult = edge.IsSendingMessage() ? $"A[{edge.AgentIndex}].{edge.SendMessageServer}.{edge.SendMessage}, {Name}.{edge.Target}" : $"{Name}.{edge.Target}";
                sb.Append($"    {{A[{edge.AgentIndex}].{Name}.{edge.ReceiveMessage}, {Name}.{edge.Source}}} -> {{{actionResult}}}");
                if (i != Edges.Count - 1) sb.Append(',');
                sb.AppendLine();
            }
            sb.AppendLine("};");

            return sb.ToString();
        }
    }
}
