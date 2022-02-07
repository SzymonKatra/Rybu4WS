using Rybu4WS.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rybu4WS.StateMachine.Composed
{
    public class ComposedStateMachine
    {
        private ListVariableValueEqualityComparer _listStatePairComparer = new ListVariableValueEqualityComparer();

        public string Name { get; set; }

        public ComposedState InitNode { get; set; }

        public List<ComposedState> Nodes { get; set; } = new List<ComposedState>();

        public List<ComposedAction> Edges { get; set; } = new List<ComposedAction>();

        public int[] RequiredAgents { get; set; }

        public ComposedState GetOrCreateNode(Dictionary<int, State> baseNodes, List<VariableValue> states, out bool isNew)
        {
            ComposedState result = null;
            foreach (var item in Nodes)
            {
                if (!CompareStates(item.States, states)) continue;

                bool isMatch = true;
                foreach (var kvp in baseNodes)
                {
                    if (!item.Agents.ContainsKey(kvp.Key)) throw new Exception("Existing nodes does not contain the same amout of agents");

                    var baseRef = item.Agents[kvp.Key].BaseNodeReference;
                    //if (baseRef.Caller != kvp.Value.Caller || baseRef.CodeLocation != kvp.Value.CodeLocation || baseRef.IsPending != kvp.Value.IsPending)
                    if (baseRef != kvp.Value)
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
                result = new ComposedState() { States = new List<VariableValue>(states) };
                foreach (var (agentIndex, baseNode) in baseNodes)
                {
                    var agentState = new ComposedState.AgentState()
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

        public ComposedAction GetOrCreateEdge(int agentIndex, ComposedState source, ComposedState target, string receiveMessage, (string serverName, string message) sendMessage, TimedDelay delay, out bool isNew)
        {
            var edge = Edges.SingleOrDefault(x =>
                x.AgentIndex == agentIndex &&
                x.Source == source &&
                x.Target == target &&
                x.ReceiveMessage == receiveMessage &&
                x.SendMessage == sendMessage.message &&
                x.SendMessageServer == sendMessage.serverName &&
                x.Delay == delay);

            isNew = edge == null;
            if (isNew)
            {
                edge = new ComposedAction()
                {
                    AgentIndex = agentIndex,
                    Source = source,
                    Target = target,
                    ReceiveMessage = receiveMessage,
                    SendMessage = sendMessage.message,
                    SendMessageServer = sendMessage.serverName,
                    Delay = delay
                };
                Edges.Add(edge);
                source.Agents[agentIndex].OutEdges.Add(edge);
                target.Agents[agentIndex].InEdges.Add(edge);
            }

            return edge;
        }

        private bool CompareStates(List<VariableValue> a, List<VariableValue> b)
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
                sb.Append($"    {{A[{edge.AgentIndex}].{Name}.{edge.ReceiveMessage}, {Name}.{edge.Source}}} -> {edge.Delay?.ToDedan()}{{{actionResult}}}");
                if (i != Edges.Count - 1) sb.Append(',');
                sb.AppendLine();
            }
            sb.AppendLine("};");

            return sb.ToString();
        }
    }
}
