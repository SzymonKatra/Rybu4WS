using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServCompEvol.Algorithm
{
    public class Graph
    {
        public List<Node> Nodes { get; set; } = new List<Node>();
        public List<Edge> Edges { get; set; } = new List<Edge>();

        public Node AddNode(string nodeName)
        {
            var node = new Node() { Name = nodeName };
            Nodes.Add(node);
            return node;
        }

        public Node this[string nodeName]
        {
            get => Nodes.Single(x => x.Name == nodeName);
        }

        public void AddEdge(string sourceNodeName, string targetNodeName, Agent allowedAgent = null)
        {
            var allowedAgents = allowedAgent != null ? new Agent[] { allowedAgent } : null;
            AddEdge(sourceNodeName, targetNodeName, allowedAgents);
        }

        public void AddEdge(string sourceNodeName, string targetNodeName, Agent[] allowedAgents)
        {
            AddEdge(this[sourceNodeName], this[targetNodeName], allowedAgents);
        }

        public void AddEdge(Node source, Node target, Agent[] onlyForAgents)
        {
            var edge = new Edge(source, target, onlyForAgents);
            Edges.Add(edge);

            source.OutEdges.Add(edge);
            target.InEdges.Add(edge);
        }
    }
}
