﻿using Rybu4WS.Language;
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

        public int? AgentIndex { get; set; }

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

        public Node GetOrCreateNode(List<StatePair> states, string caller, CodeLocation? codeLocation, bool isPending = false)
        {
            var node = Nodes.FirstOrDefault(x => CompareStates(x.States, states) && x.Caller == caller && x.CodeLocation == codeLocation && x.IsPending == isPending);

            if (node == null)
            {
                node = new Node()
                {
                    States = new List<StatePair>(states),
                    Caller = caller,
                    CodeLocation = codeLocation,
                    IsPending = isPending
                };
                this.Nodes.Add(node);
            }

            return node;
        }

        public Edge CreateEdge(Node source, Node target, string receiveMessage) => CreateEdge(source, target, receiveMessage, (null, null));

        public Edge CreateEdge(Node source, Node target, string receiveMessage, (string serverName, string message) sendMessage)
        {
            var edge = Edges.FirstOrDefault(x =>
                x.Source == source &&
                x.Target == target &&
                x.ReceiveMessage == receiveMessage &&
                x.SendMessageServer == sendMessage.serverName &&
                x.SendMessage == sendMessage.message);

            if (edge == null)
            {
                edge = new Edge()
                {
                    Source = source,
                    Target = target,
                    ReceiveMessage = receiveMessage,
                    SendMessageServer = sendMessage.serverName,
                    SendMessage = sendMessage.message
                };
                this.Edges.Add(edge);
                source.OutEdges.Add(edge);
                target.InEdges.Add(edge);
            }

            return edge;
        }

        private static readonly ListStatePairEqualityComparer _listStatePairComparer = new ListStatePairEqualityComparer();
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
            var agentIteratorStr = AgentIndex == null ? "<j=1..N>" : "";
            var agentIndexStr = AgentIndex == null ? "j" : AgentIndex.ToString();
            for (int i = 0; i < Edges.Count; i++)
            {
                var edge = Edges[i];
                var actionResult = edge.IsSendingMessage() ? $"A[{agentIndexStr}].{edge.SendMessageServer}.{edge.SendMessage}, {Name}.{edge.Target}" : $"{Name}.{edge.Target}";
                sb.Append($"    {agentIteratorStr}{{A[{agentIndexStr}].{Name}.{edge.ReceiveMessage}, {Name}.{edge.Source}}} -> {edge.Delay?.ToDedan()}{{{actionResult}}}");
                if (i != Edges.Count - 1) sb.Append(',');
                sb.AppendLine();
            }
            sb.AppendLine("};");

            return sb.ToString();
        }
    }
}
